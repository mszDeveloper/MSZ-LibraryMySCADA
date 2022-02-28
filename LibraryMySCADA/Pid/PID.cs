using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfLibMszControl.Pid
{
    public class PID : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPrChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //--------------------------------
        public double _StartPoint;
        public double StartPoint { get => _StartPoint; set { _StartPoint = value; OnPrChanged(); } }
         //--------------------------------
        public double _ValuePID;
        public double ValuePID
        {
            get { return _ValuePID; }
            set { _ValuePID = value; OnPrChanged(); }
        }
        //-----Составляющие пида---------
        public double _DifK;
        public double DifK
        {
            get { return _DifK; }
            set { _DifK = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _IntegralK;
        public double IntegralK
        {
            get { return _IntegralK; }
            set { _IntegralK = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _PropK;
        public double PropK
        {
            get { return _PropK; }
            set { _PropK = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _Dif_Work;
        public double Dif_Work
        {
            get { return _Dif_Work; }
            set { _Dif_Work = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _Integral_Work;
        public double Integral_Work
        {
            get { return _Integral_Work; }
            set { _Integral_Work = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _Prop_Work;
        public double Prop_Work
        {
            get { return _Prop_Work; }
            set { _Prop_Work = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _S;
        public double S//составляющая интегрального регулятора
        {
            get { return _S; }
            set { _S = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _En;
        public double En //составляющая дифференциального звена
        {
            get { return _En; }
            set { _En = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _e;
        public double e //e - рассоглосования (T_UST - OS_PID)
        {
            get { return _e; }
            set { _e = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _TimeOutPid;
        public double TimeOutPid //Задержка вклчения в секундах
        {
            get { return _TimeOutPid; }
            set { _TimeOutPid = value; OnPrChanged(); }
        }

        public double _TimeOutPidOff;
        public double TimeOutPidOff //Задержка выключения в секундах    
        {
            get { return _TimeOutPidOff; }
            set { _TimeOutPidOff = value; OnPrChanged(); }
        }

        //--------------------------------
        public double _TimePid;
        public double TimePid
        {
            get { return _TimePid; }
            set { _TimePid = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _MaxPID;
        public double MaxPID
        {
            get { return _MaxPID; }
            set { _MaxPID = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _MinPID;
        public double MinPID
        {
            get { return _MinPID; }
            set { _MinPID = value; OnPrChanged(); }
        }
        //--------------------------------
        public bool _LimitCalc = true;
        public bool LimitCalc { get => _LimitCalc; set { _LimitCalc = value; OnPrChanged(); } } //ограничивать пид (True - да)
         //--------------------------------
        public bool _AlwaysPositive;
        public bool AlwaysPositive { get => _AlwaysPositive; set { _AlwaysPositive = value; OnPrChanged(); } } //всегда положительный пид (True - да)
        //--------------------------------
        public double _T_UST; //уставка
        public double T_UST
        {
            get { return _T_UST; }
            set { _T_UST = value; OnPrChanged(); }
        }
        //--------------------------------
        public double _OS_PID;
        public double OS_PID
        {
            get { return _OS_PID; }
            set { _OS_PID = value; OnPrChanged(); }
        }
        //--------------------------------

        public bool _on_DifK;
        public bool on_DifK
        {
            get { return _on_DifK; }
            set { _on_DifK = value; OnPrChanged(); }
        }

        public bool _on_IntegK;
        public bool on_IntegK
        {
            get { return _on_IntegK; }
            set { _on_IntegK = value; OnPrChanged(); }
        }

        public bool _on_PropK;
        public bool CoolingHeat;

        public bool on_PropK
        {
            get { return _on_PropK; }
            set { _on_PropK = value; OnPrChanged(); }
        }


    }
}
