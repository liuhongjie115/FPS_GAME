using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    public class TimerManager
    {
        private class TimerVO
        {
            public object key;
            public float interval;
            public bool isOnce;
            public double triggerTime;
            public Action mainCallBack;
            public Delegate customCallBack;
            public object[] cbParams;
        }

        private static Dictionary<object, TimerVO> tempDict = new Dictionary<object, TimerVO>();
        private static Dictionary<object, TimerVO> curDict = new Dictionary<object, TimerVO>();
        private static Dictionary<object, TimerVO> delOnceDict = new Dictionary<object, TimerVO>();
        private static Dictionary<object, bool> delReadyDict = new Dictionary<object, bool>();

        private static List<Action> saveCBActions = new List<Action>();
        private static object saveLock = new object();

        private static double curTime;
        private static float intervalFrame;
        private static float intervalSecond = 1;
        private static int keyAutoAdd = 0;

        static TimerManager()
        {
            System.Timers.Timer time = new System.Timers.Timer();
            time.Interval = 10;
            time.Elapsed += new System.Timers.ElapsedEventHandler(Update);
            time.Enabled = true;
        }

        public static void Init()
        {
            Dispose();
            intervalFrame = 0.033f;

            keyAutoAdd = 0;
        }

        public static void Update(Object send,System.Timers.ElapsedEventArgs args)
        {
            curTime = TimeUtil.GetNowTime();
            if (delReadyDict.Count > 0)
            {
                var eNumTempDict = delReadyDict.GetEnumerator();
                while (eNumTempDict.MoveNext())
                {
                    if (curDict.ContainsKey(eNumTempDict.Current.Key))
                    {
                        curDict.Remove(eNumTempDict.Current.Key);
                    }
                    if (tempDict.ContainsKey(eNumTempDict.Current.Key))
                    {
                        tempDict.Remove(eNumTempDict.Current.Key);
                    }
                }
                delReadyDict.Clear();
            }

            if (tempDict.Count > 0)
            {
                var eNumTempDict = tempDict.GetEnumerator();
                while (eNumTempDict.MoveNext())
                {
                    var vo = eNumTempDict.Current;
                    curDict[vo.Key] = vo.Value;
                }
                tempDict.Clear();
            }

            UpdateTimerDict();
            OnSaveCallBack();
        }

        private static void UpdateTimerDict()
        {
            //Console.WriteLine("循环");
            var eNumTimerDict = curDict.GetEnumerator();
            try
            {
                while (eNumTimerDict.MoveNext())
                {
                    var vo = eNumTimerDict.Current.Value;
                    if (curTime - vo.triggerTime >= vo.interval)
                    {
                        if (vo.mainCallBack != null)
                        {
                            vo.mainCallBack.Invoke();
                        }
                        else
                        {
                            if (vo.cbParams == null || vo.cbParams.Length == 0)
                            {
                                vo.customCallBack.DynamicInvoke();
                            }
                            else
                            {
                                vo.customCallBack.DynamicInvoke(vo.cbParams);
                            }
                        }

                        if (vo.isOnce)
                        {
                            delOnceDict[vo.key] = vo;
                        }
                        else
                        {
                            vo.triggerTime = curTime;
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
            

            if (delOnceDict.Count > 0)
            {
                eNumTimerDict = delOnceDict.GetEnumerator();
                while (eNumTimerDict.MoveNext())
                {
                    if (curDict.ContainsKey(eNumTimerDict.Current.Key))
                    {
                        curDict.Remove(eNumTimerDict.Current.Key);
                    }
                }
                delOnceDict.Clear();
            }
        }

        private static void OnSaveCallBack()
        {
            if (saveCBActions.Count > 0)
            {
                for (int i = 0; i < saveCBActions.Count; i++)
                {
                    saveCBActions[i].Invoke();
                }
                saveCBActions.Clear();
            }
        }

        public static void AddTimerFrame(object key, Action cb)
        {
            if (HasTimer(key) == false)
            {
                AddTimerPrivate(new TimerVO()
                {
                    key = key,
                    interval = intervalFrame,
                    mainCallBack = cb
                });
            }

        }

        public static void AddTimerSecond(object key, Action cb)
        {
            if (HasTimer(key) == false)
            {
                AddTimerPrivate(new TimerVO()
                {
                    key = key,
                    interval = intervalSecond,
                    mainCallBack = cb
                });
            }
        }

        public static object AddTimerInterval(object key, Delegate cb, float interval, params object[] args)
        {
            if (HasTimer(key) == false)
            {
                AddTimerPrivate(new TimerVO()
                {
                    key = key,
                    interval = interval,
                    customCallBack = cb,
                    cbParams = args
                });
            }
            return key;
        }

        public static int SetDelay(Delegate cb, float interval, params object[] args)
        {
            var key = ++keyAutoAdd;
            AddTimerPrivate(new TimerVO()
            {
                key = key,
                triggerTime = curTime,
                interval = interval,
                customCallBack = cb,
                cbParams = args,
                isOnce = true
            });
            return key;
        }

        private static void AddTimerPrivate(TimerVO vo)
        {
            tempDict[vo.key] = vo;
        }


        public static bool HasTimer(object key)
        {
            return tempDict.ContainsKey(key) || curDict.ContainsKey(key);
        }


        public static void DelTimer(object key)
        {
            if (key != null)
            {
                delReadyDict[key] = true;
            }
        }

        public static void SaveCallBack(Action action)
        {
            lock (saveLock)
            {
                saveCBActions.Add(action);
            }
        }

        public static double GetTimePTime()
        {
            return curTime;
        }

        public static void Dispose()
        {
            tempDict.Clear();
            curDict.Clear();
            delReadyDict.Clear();
            curTime = 0;
        }
    }
}
