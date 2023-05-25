using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
#if UNITY_IOS
using UnityEngine.iOS; //引入命名空间
#endif
public class UnityToIOSManager : MonoBehaviour
{
    static UnityToIOSManager mInstance;
    static long lastTime;

    int PushIndex;//推送
    string PushText; //推送信息

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//游戏运行时禁止休眠，游戏在前台时可以保持屏幕长亮
        if (mInstance == null)
            mInstance = this;
#if UNITY_IOS
        //先清空本地消息，然后注册
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(NotificationType.Badge | NotificationType.Alert | NotificationType.Sound);
        CleanNotification();
#endif

        //DontDestroyOnLoad(this);

    }
    //当有多条推送时，每次进入游戏时更换推送消息
    private void Push()
    {

        PushText = "";
    }



    public static void SpecialNotification(string message, int Mouths,int Days,int Hours, bool isRepeatDay)
    {
        int year = System.DateTime.Now.Year;

        
        System.DateTime newDate = new System.DateTime(year, Mouths, Days, Hours, 0, 0);
        NotificationMessage(message, newDate, isRepeatDay);

    }



    //后台消息通知
    //本地推送，每日重复
    public static void NotificationMessage(string message, int hour, bool isRepeatDay)
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newDate = new System.DateTime(year, month, day, hour, 0, 0);
        NotificationMessage(message, newDate, isRepeatDay);

    }

    //本地推送 你可以传入一个固定的推送时间
    public static void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
    {
#if UNITY_IOS
        if (isRepeatDay && newDate <= System.DateTime.Now)
        {
            newDate = newDate.AddDays(1);
        }
        //推送时间需要大于当前时间
        if (newDate > System.DateTime.Now)
        {
            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
            localNotification.fireDate = newDate;
            localNotification.alertBody = message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.hasAction = true;
            if (isRepeatDay)
            {
                //是否每天定期循环
                localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
            }
            localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
        }
#endif
    }


    void OnApplicationPause(bool paused)
    {
        //程序进入后台时
        if (paused)
        {
            //Push();


            //每天中午12点
            NotificationMessage("Come back to check our new nails collections and get some rewards!", 12, true);
            //每天晚上20点
            NotificationMessage("Good evening beautiful! Paint these awesome nails and share with your friends!", 20, true);

            //当积累约1000赞时发送推送
            //NotificationMessage("Check out your latest reward!", System.DateTime.Now.AddSeconds(15300), false);
            NotificationMessage("Check out your latest reward!", System.DateTime.Now.AddSeconds(3600), false);

            //圣诞节
            SpecialNotification("Ho Ho Ho! Santa brings these gorgeous nails for you! Hello dear we wish you a merry nails day!", 12, 25, 8, false);

            //时间戳
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            lastTime = Convert.ToInt64(ts.TotalSeconds);
        }
        else
        {
            //程序从后台进入前台时
            CleanNotification();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long nowTime = Convert.ToInt64(ts.TotalSeconds);
            if (nowTime - lastTime > 1800)
            {
                //这个是判断程序退出到后台超出多久之后
                //然后进行其他操作，播放广告，或是提醒什么的

                UIDailyControl.Intance.ShowDailyReward();

            }
        }

    }

    //清空所有本地消息
    void CleanNotification()
    {
#if UNITY_IOS
        UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification();
        l.applicationIconBadgeNumber = -1;
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(l);
        Invoke("WaitOneFrameClear", 0);
#endif
    }
    //延迟一帧执行，不然没法清理
    void WaitOneFrameClear()
    {
#if UNITY_IOS
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
#endif
    }

}
