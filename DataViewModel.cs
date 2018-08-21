using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Data.Model;
using System.Windows.Input;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Controls;
using FCCT.View;
namespace FCCT.ViewModel
{
    public class CMainViewModel : INotifyPropertyChanged
    {

        public ICommand m_TabSelectChangedCommand{get;set;}
        private CRATSet m_RATSet;
        public CRATSet M_RATSet 
        {
            get
            {
                return m_RATSet;
            }
            set
            {
                m_RATSet = value;
            }
        }

        private CRATViewModel m_RATView;
        public CRATViewModel M_RATView
        {
            get
            {
                return m_RATView;
            }
            set
            {
                m_RATView = value;
                NotifyPropertyChanged("M_RATView");
            }
        }
        public TabItem M_curTabItem//当前TAB指向的Item
        {
            get;
            set;
        }
        public CMainViewModel()
        {
            try
            {
                M_RATSet = XmlParse.DeserializeFromXml<CRATSet>("TEST.xml");
            }
            catch (Exception e)
            {
                MessageBox.Show("请检查XML配置文件:" + e.Message,"Fatal Error!",MessageBoxButton.OK,MessageBoxImage.Error);
                Environment.Exit(0);
            }
            //m_RATView = new CRATViewModel(m_RATSet.M_strFormatList[(int)RAT.NR]);
            //M_curTabItem = new TabItem();
            try{
                M_RATView = new CRATViewModel(m_RATSet.M_strFormatList.Find(s => s.M_strRAT == "NR"));
            }
            catch (Exception e)
            {
                MessageBox.Show("请检查XML配置文件:" + e.Message,"Fatal Error!",MessageBoxButton.OK,MessageBoxImage.Error);
                Environment.Exit(0);
            }
            m_TabSelectChangedCommand = new CDelegateCommand(TabSelectChangedCommand,CanExeTabSelectChangedCommand);

        }
        public void TabSelectChangedCommand(object param)//tab标签页转换时，更换一个制式
        {
            //获取Tab栏选中Item的Header名称，在FormatList中寻找对应制式  
            //M_RATView = new CRATViewModel(m_RATSet.M_strFormatList.Find(s => s.M_strRAT == this.M_curTabItem.Header.ToString()));
            
            //0803-10:41-log-每次不再构造新的RATView对象  
            if (M_curTabItem.Header.ToString() == this.M_RATView.M_RAT.M_strRAT)
                return;
            try
            {
                M_RATView.ChangeRAT(m_RATSet.M_strFormatList.Find(s => s.M_strRAT == this.M_curTabItem.Header.ToString()));
            }
            catch (Exception e)
            {
                MessageBox.Show("请检查XML配置文件:" + e.Message, "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }
        public bool CanExeTabSelectChangedCommand(object param)//tab标签页转换时，更换一个制式
        {

            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class CRATViewModel : INotifyPropertyChanged
    {
        private CRAT m_RAT;
        public CRAT M_RAT
        {
            get
            {
                return m_RAT;
            }
            set
            {
                m_RAT = value;
                NotifyPropertyChanged("M_RAT");
            }
        }
        private List<CBandInfo> m_curBandList;
        public List<CBandInfo> M_curBandList
        {
            get
            {
                return m_curBandList;
            }
            set
            {
                m_curBandList = value;
                NotifyPropertyChanged("M_curBandList");
            }
        }

        private CBandInfo m_curBandInfo;
        public CBandInfo M_curBandInfo
        {
            get
            {
                return m_curBandInfo;
            }
            set
            {
                m_curBandInfo = value;
                NotifyPropertyChanged("M_curBandInfo");
            }
        }
        public ICommand m_ComboBoxSelectChangedCommand { get; set; }
        public void ComboBoxSelectChangedCommand(object param)//ComboBox标签页转换时，更换一个制式
        {
            this.M_BandInfoView = new CBandInfoViewModel(M_curBandInfo, this.M_RAT.M_PowerCtrLev);
            //刷新当前频率、信道信息
            this.M_DScrollBarValue = 0;
            ScrollCommand(null);
            try
            {
                if (this.M_RAT.M_strRAT == "GSM")
                {
                    M_curPowerClass = M_BandInfoView.M_BandInfo.M_PowerInfo.M_PowerItem[0].M_strClass;
                }
                M_curSCS = "Refresh";//提供一个刷新事件,对于NR协议，清空所有的已选带宽，对于其他协议，刷新带宽值
                if (M_BandInfoView.M_BandInfo.M_BandWidth[0] == null || M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strSCS == null || M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strSCS == "")
                {
                    M_curSCS = "RefreshOther";
                }//非NR协议不需要SCS这一项
                else
                {
                    M_curSCS = M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strSCS;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+"!\r\n请检查XML配置文件！BandWidth或PowerInfo字段缺少重要信息！", "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }


        }
        public bool CanExeComboBoxSelectChangedCommand(object param)//ComboBox标签页转换时，更换一个制式
        {

            return true;
        }
        private CBandInfoViewModel m_BandInfoView;//频带信息视图模型
        public CBandInfoViewModel M_BandInfoView
        {
            get
            {
                return m_BandInfoView;
            }
            set
            {
                m_BandInfoView = value;
                NotifyPropertyChanged("M_BandInfoView");
            }
        }

        //CRATViewModel实例只会构造一次，发生在窗口创建的时候，也就是每次启动程序创建后，该实例一直存在，更换频带只会更换其成员变量
        public CRATViewModel(CRAT curRAT)
        {
            m_ComboBoxSelectChangedCommand = new CDelegateCommand(ComboBoxSelectChangedCommand, CanExeComboBoxSelectChangedCommand);
            M_curBandInfo = new CBandInfo();//用于绑定当前的频带
            M_curBandList = new List<CBandInfo>();//用于绑定该协议的频带列表
            M_RAT = new CRAT();
            M_RAT = curRAT;//指明当前协议，并存放所有数据信息
            try
            {
                M_curBandInfo = M_RAT.M_BandList[0];//初始化的时候，指向第一个频带
                M_curBandList = M_RAT.M_BandList;
                //M_curBandInfo = this.M_curBandList.Find(s => s.M_HisiInfo.M_usIndex==this.M_curBandList.Min(x => x.M_HisiInfo.M_usIndex)); 
                //this.M_BandInfoView.M_BandInfo = M_curBandInfo;
                M_BandInfoView = new CBandInfoViewModel(M_curBandInfo, this.M_RAT.M_PowerCtrLev);
            }
            catch (Exception e)
            {
                MessageBox.Show("请检查XML配置文件:" + e.Message, "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            //this.M_BandInfoView.ConfigFixedInfo();
            m_ScrollCommand = new CDelegateCommand(ScrollCommand, CanExeScrollCommand);
            //m_DlChannelKeyUpCommand = new CDelegateCommand(DlChannelKeyUpCommand, CanExeDlChannelKeyUpCommand);
            //刷新当前频率、信道信息
            ComboBoxSelectChangedCommand(null);
            /*
            M_strFreDl = (M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmin).ToString("f3");
            M_strFreUl = (M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmin).ToString("f3");
            M_UChanDl = M_BandInfoView.M_BandInfo.M_ChanInfo.M_UDlChanMin;
            M_UChanUl = M_BandInfoView.M_BandInfo.M_ChanInfo.M_UUlChanMin;*/
            this.M_DScrollBarValue = 0;
            ScrollCommand(null);
            M_strWindowTitle = "频率信道计算工具   " + M_RAT.M_strRAT + "(Release:" + M_RAT.M_strRelease + ")";
        }
        public void ChangeRAT(CRAT curRAT)
        {
            m_ComboBoxSelectChangedCommand = new CDelegateCommand(ComboBoxSelectChangedCommand, CanExeComboBoxSelectChangedCommand);
            M_RAT = curRAT;
            M_curBandInfo = M_RAT.M_BandList[0];
            M_curBandList = M_RAT.M_BandList;
            //M_curBandInfo = this.M_curBandList.Find(s => s.M_HisiInfo.M_usIndex==this.M_curBandList.Min(x => x.M_HisiInfo.M_usIndex)); 
            
            //this.M_BandInfoView.M_BandInfo = M_curBandInfo;
            M_BandInfoView = new CBandInfoViewModel(M_curBandInfo, this.M_RAT.M_PowerCtrLev);
            M_strWindowTitle = "频率信道计算工具   " + M_RAT.M_strRAT + "(Release:" + M_RAT.M_strRelease + ")";

        }
        private bool FindBand(UInt32 Channel,bool Mode)//在该制式内检查输入的信道属于什么频段
        {
            bool FindFlag = false;
            try
            {
                foreach (CBandInfo tempBandInfo in M_RAT.M_BandList)
                {
                    if (Mode)//true表明DL，false表明UL
                    {
                        if (Channel >= tempBandInfo.M_ChanInfo.M_UDlChanMin && Channel <= tempBandInfo.M_ChanInfo.M_UDlChanMax)
                        {
                            M_curBandInfo = tempBandInfo;
                            FindFlag = true;
                            break;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (Channel >= tempBandInfo.M_ChanInfo.M_UUlChanMin && Channel <= tempBandInfo.M_ChanInfo.M_UUlChanMax)
                        {
                            M_curBandInfo = tempBandInfo;
                            FindFlag = true;
                            break;
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ".\r\n因为缺少一些必要的数据，该制式的查询功能已被禁用。请检查XML文件。", "警告!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return FindFlag;
        }

        private bool FindBand(float Freq, bool Mode)//在该制式内检查输入的信道属于什么频段
        {
            bool FindFlag = false;
            try
            {
                foreach (CBandInfo tempBandInfo in M_RAT.M_BandList)
                {
                    if (Mode)//true表明DL，false表明UL
                    {
                        if (Freq >= tempBandInfo.M_Frequency.M_FDlmin && Freq <= tempBandInfo.M_Frequency.M_FDlmax)
                        {
                            M_curBandInfo = tempBandInfo;
                            FindFlag = true;
                            break;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (Freq >= tempBandInfo.M_Frequency.M_FUlmin && Freq <= tempBandInfo.M_Frequency.M_FUlmax)
                        {
                            M_curBandInfo = tempBandInfo;
                            FindFlag = true;
                            break;
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ".\r\n因为缺少一些必要的数据，该制式的查询功能已被禁用。请检查XML文件。", "警告!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return FindFlag;
        }
        private String m_strWindowTitle;
        public String M_strWindowTitle
        {
            get
            {
                return m_strWindowTitle;
            }
            set
            {
                m_strWindowTitle = value;
                NotifyPropertyChanged("M_strWindowTitle");
            }
        }
        //当前上行频率
        private String m_strFreUl;
        public String M_strFreUl
        {
            get
            {
                return m_strFreUl;
            }
            set
            {
                m_strFreUl = value;
                NotifyPropertyChanged("M_strFreUl");
                NotifyPropertyChanged("M_strDisplayFreUl");
            }
        }
        //当前下行频率
        private String m_strFreDl;
        public String M_strFreDl
        {
            get
            {
                return m_strFreDl;
            }
            set
            {
                m_strFreDl = value;
                NotifyPropertyChanged("M_strFreDl");
                NotifyPropertyChanged("M_strDisplayFreDl");
            }
        }
        //当前上行信道
        private UInt32 m_UChanUl;
        public UInt32 M_UChanUl
        {
            get
            {
                return m_UChanUl;
            }
            set
            {
                m_UChanUl = value;
                NotifyPropertyChanged("M_UChanUl");
                NotifyPropertyChanged("M_UDisplayChanUl");
            }
        }
        //当前下行信道
        private UInt32 m_UChanDl;
        public UInt32 M_UChanDl
        {
            get
            {
                return m_UChanDl;
            }
            set
            {
                m_UChanDl = value;
                NotifyPropertyChanged("M_UChanDl");
                NotifyPropertyChanged("M_UDisplayChanDl");
            }
        }
        //用户输入上行信道
        public UInt32 M_UDisplayChanUl
        {
            get
            {
                return M_UChanUl;
            }
            set
            {
                if (this.M_BandInfoView.IsChannelIn(value, false))//false代表当前是UL的信道号,检查是否在当前频段内
                {
                    M_DScrollBarValue = (value - this.M_BandInfoView.M_BandInfo.M_ChanInfo.M_UUlChanMin) / this.M_BandInfoView.M_usChanStepSize;
                }
                else
                {
                    if (this.FindBand(value, false))//不再频段内，检查是否制式中存在这个信道号
                    {
                        M_DScrollBarValue = (value - this.M_BandInfoView.M_BandInfo.M_ChanInfo.M_UUlChanMin) / this.M_BandInfoView.M_usChanStepSize;
                    }
                    //注意有的协议中某一信道号可能出现在多个频段中，软件只返回XML文件顺序中的包含该信道的第一个频段

                }
                NotifyPropertyChanged("M_UDisplayChanUl");
            }
        }
        //用户输入下行信道
        public UInt32 M_UDisplayChanDl//同上
        {
            get
            {
                return M_UChanDl;
            }
            set
            {
                if (this.M_BandInfoView.IsChannelIn(value, true))//false代表当前是UL的信道号
                {
                    M_DScrollBarValue = (value - this.M_BandInfoView.M_BandInfo.M_ChanInfo.M_UDlChanMin) / this.M_BandInfoView.M_usChanStepSize;
                }
                else
                {
                    if (this.FindBand(value, true))
                    {
                        M_DScrollBarValue = (value - this.M_BandInfoView.M_BandInfo.M_ChanInfo.M_UDlChanMin) / this.M_BandInfoView.M_usChanStepSize;
                    }

                }
                NotifyPropertyChanged("M_UDisplayChanDl");
            }
        }
        //用户输入上行频率
        public String M_strDisplayFreUl
        {
            get
            {
                return M_strFreUl;
            }
            set
            {
                try
                {

                    if (this.M_BandInfoView.IsFreqIn(float.Parse(value), false))//false代表当前是UL的频率,检查是否在当前频段内
                    {
                        M_DScrollBarValue = (double.Parse(value) - this.M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmin) * 1000 / this.M_BandInfoView.M_usChanSpacing;
                    }
                    else
                    {
                        if (this.FindBand(float.Parse(value), false))//不再频段内，检查是否制式中存在这个频率
                        {
                            M_DScrollBarValue = (double.Parse(value) * -this.M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmin) * 1000 / this.M_BandInfoView.M_usChanSpacing;
                        }
                        //注意有的协议中某一频率可能出现在多个频段中，软件只返回XML文件顺序中的包含该信道的第一个频段

                    }
                }
                catch (FormatException)
                {}
                catch (OverflowException)
                {}
                NotifyPropertyChanged("M_UDisplayFreUl");
            }
        }
        //用户输入下行频率
        public String M_strDisplayFreDl//同上
        {
            get
            {
                return M_strFreDl;
            }
            set
            {
                try{           
                    if (this.M_BandInfoView.IsFreqIn(float.Parse(value), true))//false代表当前是UL的信道号
                    {
                        M_DScrollBarValue = (double.Parse(value) - this.M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmin)*1000 / this.M_BandInfoView.M_usChanSpacing;
                    }
                    else
                    {
                        if (this.FindBand(float.Parse(value), true))
                        {
                            M_DScrollBarValue = (double.Parse(value) * - this.M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmin )*1000 / this.M_BandInfoView.M_usChanSpacing;
                        }

                    }
                }
                catch (FormatException)
                {}
                catch (OverflowException)
                {}
                NotifyPropertyChanged("M_UDisplayFreDl");
            }
        }        

        private double m_DScrollBarValue;//滚动条位置
        public double M_DScrollBarValue
        {
            get
            {
                return m_DScrollBarValue;
            }
            set
            {
                m_DScrollBarValue = value;
                NotifyPropertyChanged("M_DScrollBarValue");
            }
        }
        public ICommand m_ScrollCommand { get; set; }
        public void ScrollCommand(object param)//滚动条变化
        {
            try
            {
                UInt16 usTempScrollBarValue = (UInt16)Math.Round(M_DScrollBarValue, 0);
                float FcurFreDl = (M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmin + (float)usTempScrollBarValue * M_BandInfoView.M_BandInfo.M_ChanInfo.M_usChanSpacing / 1000);
                float FcurFreUl = (M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmin + (float)usTempScrollBarValue * M_BandInfoView.M_BandInfo.M_ChanInfo.M_usChanSpacing / 1000);
                M_strFreDl = FcurFreDl.ToString("f3");
                M_strFreUl = FcurFreUl.ToString("f3");
                M_UChanDl = M_BandInfoView.M_BandInfo.M_ChanInfo.M_UDlChanMin + (UInt32)usTempScrollBarValue * M_BandInfoView.M_usChanStepSize;
                M_UChanUl = M_BandInfoView.M_BandInfo.M_ChanInfo.M_UUlChanMin + (UInt32)usTempScrollBarValue * M_BandInfoView.M_usChanStepSize;
                //频带展示数据
                double tempFreLeft = Math.Min((double)M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmin, (double)M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmin);
                if (tempFreLeft == 0)
                {
                    tempFreLeft = Math.Max((double)M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmin, (double)M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmin);
                }
                double tempFreRight = Math.Max((double)M_BandInfoView.M_BandInfo.M_Frequency.M_FDlmax, (double)M_BandInfoView.M_BandInfo.M_Frequency.M_FUlmax);
                M_DLeftXPosition = (tempFreLeft < 6000) ? tempFreLeft * 0.06 : 380 + (tempFreLeft - 24000) * 0.002;
                M_DRightXPosition = (tempFreRight < 6000) ? tempFreRight * 0.06 : 380 + (tempFreRight - 24000) * 0.002;
                M_DBandBorderWidth = M_DRightXPosition - M_DLeftXPosition;
                M_ThickBandBorderMargin = new Thickness(M_DLeftXPosition, 15, 0, 0);
                M_CurFrePositionDl = 80 + (FcurFreDl - tempFreLeft) * 300 / (tempFreRight - tempFreLeft);
                M_CurFrePositionUl = 80 + (FcurFreUl - tempFreLeft) * 300 / (tempFreRight - tempFreLeft);
                M_ThickUlDlDifLabelMargin = new Thickness(Math.Min(M_CurFrePositionDl, M_CurFrePositionUl) + Math.Abs(M_CurFrePositionDl - M_CurFrePositionUl) / 2 - 30, 46, 0, 0);
                M_strUlDlDifLabelContent = Math.Abs(FcurFreDl - FcurFreUl).ToString("f0") + "MHz";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ".\r\n因为缺少一些必要的数据，一些功能可能被禁用。请检查XML文件后重启软件。", "警告!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public bool CanExeScrollCommand(object param)//滚动条变化
        {
            return true;
        }
        private double m_DLeftXPosition;
        public double M_DLeftXPosition
        {
            get
            {
                return m_DLeftXPosition;
            }
            set
            {
                m_DLeftXPosition = value;
                NotifyPropertyChanged("M_DLeftXPosition");
            }
        }
        private double m_DRightXPosition;
        public double M_DRightXPosition
        {
            get
            {
                return m_DRightXPosition;
            }
            set
            {
                m_DRightXPosition = value;
                NotifyPropertyChanged("M_DRightXPosition");
            }
        }
        private double m_DBandBorderWidth;
        public double M_DBandBorderWidth
        {
            get
            {
                return m_DBandBorderWidth;
            }
            set
            {
                m_DBandBorderWidth = value;
                NotifyPropertyChanged("M_DBandBorderWidth");
            }
        }
        private Thickness m_ThickBandBorderMargin;
        public Thickness M_ThickBandBorderMargin
        {
            get
            {
                return m_ThickBandBorderMargin;
            }
            set
            {
                m_ThickBandBorderMargin = value;
                NotifyPropertyChanged("M_ThickBandBorderMargin");
            }
        }
        
        private String m_strUlDlDifLabelContent;
        public String M_strUlDlDifLabelContent
        {
            get
            {
                return m_strUlDlDifLabelContent;
            }
            set
            {
                m_strUlDlDifLabelContent = value;
                NotifyPropertyChanged("M_strUlDlDifLabelContent");
            }
        }
        private Thickness m_ThickUlDlDifLabelMargin;
        public Thickness M_ThickUlDlDifLabelMargin
        {
            get
            {
                return m_ThickUlDlDifLabelMargin;
            }
            set
            {
                m_ThickUlDlDifLabelMargin = value;
                NotifyPropertyChanged("M_ThickUlDlDifLabelMargin");
            }
        }
        
        private double m_CurFrePositionUl;
        public double M_CurFrePositionUl
        {
            get
            {
                return m_CurFrePositionUl;
            }
            set
            {
                m_CurFrePositionUl = value;
                NotifyPropertyChanged("M_CurFrePositionUl");
            }
        }
        private double m_CurFrePositionDl;
        public double M_CurFrePositionDl
        {
            get
            {
                return m_CurFrePositionDl;
            }
            set
            {
                m_CurFrePositionDl = value;
                NotifyPropertyChanged("M_CurFrePositionDl");
            }
        }

        private String m_curSCS;
        public String M_curSCS
        {
            get
            {
                return m_curSCS;
            }
            set
            {
                if (value != null)
                {
                    m_curSCS = value;
                    NotifyPropertyChanged("M_curSCS");
                }

            }
        }

        public String[] M_AllowedBW//返回的是当前需要显示的一组带宽值，对于NR协议，返回当前SCS对应的带宽，非NR协议，返回当前频段带宽
        {
            get
            {//非NR协议
                if (M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strSCS == "" || M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strSCS == null)//非NR协议
                    if (M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strBW.Split(',') == null)
                    {
                        String[] tempBW = new String[1];
                        tempBW[0] = M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strBW;
                        return tempBW;
                    }
                    else
                        return M_BandInfoView.M_BandInfo.M_BandWidth[0].M_strBW.Split(',');
                else
                {       //NR协议
                    if (M_curSCS == "Refresh")//用于刷新
                        return null;
                    CBandWidth BandWidth = M_BandInfoView.M_BandInfo.M_BandWidth.Find(s => s.M_strSCS == M_curSCS);
                    if (BandWidth == null)
                        return null;
                    else
                        return BandWidth.M_strBW.Split(',');
                }
            }
            set
            {

            }
        }
        private String m_curPowerClass;//当前选择的PowerClass
        public String M_curPowerClass
        {
            get
            {
                return m_curPowerClass;
            }
            set
            {
                if (value != null)
                {
                    m_curPowerClass = value;
                    NotifyPropertyChanged("M_curPowerClass");
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public class CBandInfoViewModel : INotifyPropertyChanged
    {
        private CBandInfo m_BandInfo;
        public CBandInfo M_BandInfo
        {
            get
            {
                return m_BandInfo;
            }
            set
            {
                m_BandInfo = value;
                NotifyPropertyChanged("M_BandInfo");
            }
        }
        public CBandInfoViewModel()
        {

        }
        public CBandInfoViewModel(CBandInfo BandInfo,List<CPowerCtrLevInfo> PowerCtrLevInfo)
        {
            M_BandInfo = BandInfo;
            if (PowerCtrLevInfo != null && PowerCtrLevInfo.Count!=0)
            {
                M_PowerCtrLev = PowerCtrLevInfo.Find(s => s.M_usPCLIndex == this.M_BandInfo.M_PowerInfo.M_usPCLIndex).M_PowerTable;
            }
        }

        private List<CPowerTable> m_PowerCtrLev;
        public List<CPowerTable> M_PowerCtrLev
        {
            get
            {
                return m_PowerCtrLev;
            }
            set
            {
                m_PowerCtrLev = value;
                NotifyPropertyChanged("M_PowerCtrLev");
            }

        }

        private int m_BandPosition;//频带位置
        public int M_BandPosition
        {
            get
            {
                return m_BandPosition;
            }
            set
            {
                m_BandPosition = value;
                NotifyPropertyChanged("M_BandPosition");
            }
        }
        public UInt32 M_UChanNum//频段内信道数量
        {
            get
            {
                return (M_BandInfo.M_ChanInfo.M_UDlChanMax - M_BandInfo.M_ChanInfo.M_UDlChanMin) / M_usChanStepSize + 1;
            }
            set
            {

            }
        }
        public UInt32 M_UScrollLength
        {
            get
            {
                return M_UChanNum - 1;
            }
        }

        public UInt16 M_usChanStepSize
        {
            get
            {
                return M_BandInfo.M_ChanInfo.M_usChanStepSize;
            }
            set
            {

            }
        }
        public UInt16 M_usChanSpacing
        {
            get
            {
                return M_BandInfo.M_ChanInfo.M_usChanSpacing;
            }
            set
            {

            }
        }
        public String M_strDLMinInfo
        {
            get
            {
                return this.M_BandInfo.M_Frequency.M_FDlmin.ToString("f3") + "(" + this.M_BandInfo.M_ChanInfo.M_UDlChanMin.ToString() + ")";
            }
        }
        public String M_strDLMLInfo
        {
            get
            {
                if (M_UChanNum % 2 != 0)
                    return (M_BandInfo.M_Frequency.M_FDlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * M_UScrollLength / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UDlChanMin + M_UScrollLength * M_usChanStepSize / 2).ToString() + ")";
                else 
                {
                    return (M_BandInfo.M_Frequency.M_FDlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * (M_UChanNum - 2) / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UDlChanMin + (M_UChanNum - 2) * M_usChanStepSize / 2).ToString() + ")";
                }
            }
        }
        public String M_strDLMHInfo
        {
            get
            {
                if (M_UChanNum % 2 != 0)
                    return (M_BandInfo.M_Frequency.M_FDlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * M_UScrollLength / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UDlChanMin + M_UScrollLength * M_usChanStepSize / 2).ToString() + ")";
                else
                {
                    return (M_BandInfo.M_Frequency.M_FDlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * (M_UChanNum) / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UDlChanMin + (M_UChanNum) * M_usChanStepSize / 2).ToString() + ")";
                }
            }
        }
        public String M_strDLMaxInfo
        {
            get
            {
                return (this.M_BandInfo.M_Frequency.M_FDlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * M_UScrollLength / 1000).ToString("f3") + "(" + this.M_BandInfo.M_ChanInfo.M_UDlChanMax.ToString() + ")";
            }
        }

        public String M_strULMinInfo
        {
            get
            {
                return this.M_BandInfo.M_Frequency.M_FUlmin.ToString("f3") + "(" + this.M_BandInfo.M_ChanInfo.M_UUlChanMin.ToString() + ")";
            }
        }
        public String M_strULMLInfo
        {
            get
            {
                if (M_UChanNum % 2 != 0)
                    return (M_BandInfo.M_Frequency.M_FUlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * M_UScrollLength / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UUlChanMin + M_UScrollLength * M_usChanStepSize / 2).ToString() + ")";
                else
                {
                    return (M_BandInfo.M_Frequency.M_FUlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * (M_UChanNum - 2) / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UUlChanMin + (M_UChanNum - 2) * M_usChanStepSize / 2).ToString() + ")";
                }
            }
        }
        public String M_strULMHInfo
        {
            get
            {
                if (M_UChanNum % 2 != 0)
                    return (M_BandInfo.M_Frequency.M_FUlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * M_UScrollLength / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UUlChanMin + M_UScrollLength * M_usChanStepSize / 2).ToString() + ")";
                else
                {
                    return (M_BandInfo.M_Frequency.M_FUlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * (M_UChanNum) / 2000).ToString("f3") + "(" + (M_BandInfo.M_ChanInfo.M_UUlChanMin + (M_UChanNum) * M_usChanStepSize / 2).ToString() + ")";
                }
            }
        }
        public String M_strULMaxInfo
        {
            get
            {
                return (this.M_BandInfo.M_Frequency.M_FUlmin + (double)M_BandInfo.M_ChanInfo.M_usChanSpacing * M_UScrollLength / 1000).ToString("f3") + "(" + this.M_BandInfo.M_ChanInfo.M_UUlChanMax.ToString() + ")";
            }
        }
        public List<CPowerItem> M_MaxPowerInfo
        {
            get
            {
                return this.M_BandInfo.M_PowerInfo.M_PowerItem;
            }
        }
        public String M_strSingleDlHidden
        {
            get
            {
                if (this.M_BandInfo.M_HisiInfo.M_strIsSingleDL == "True")
                    return "Hidden";
                else 
                    return "Visible";
            }
        }
        public String M_strSingleUlHidden
        {
            get
            {
                if (this.M_BandInfo.M_HisiInfo.M_strIsSingleUL == "True")
                    return "Hidden";
                else
                    return "Visible";
            }
        }
        public String M_strDuplexHidden
        {
            get
            {
                if (this.M_BandInfo.M_HisiInfo.M_strDuplexMode == "TDD")
                    return "Hidden";
                else
                    return "Visible";
            }
        }
        public String M_strBandBarInfo//BandBar上显示的文字信息
        {
            get
            {
                String tempInfo = "";
                if (this.M_BandInfo.M_HisiInfo.M_strDuplexMode == "TDD")
                    tempInfo += "TDD";
                if (this.M_BandInfo.M_HisiInfo.M_strIsSingleUL == "True")
                    tempInfo += " UL Single";
                if (this.M_BandInfo.M_HisiInfo.M_strIsSingleDL == "True")
                    tempInfo += " DL Single";
                return tempInfo;
            }
        }
        /*
        public String M_strBandBarFreInfo//BandBar上显示的文字信息
        {
            get
            {
                return Math.Abs(this.M_BandInfo.M_Frequency.M_FDlmin - this.M_BandInfo.M_Frequency.M_FUlmin).ToString() + "MHz";
            }
        }
        */
        public bool IsChannelIn(UInt32 Channel,bool Mode)//检查输入的信道是否在频带内
        {
            //输入的Mode  true 代表DL， false 代表UL
            //返回的true代表在频带内，false表示不在频带内
            if (Mode)
            {
                //DL
                if (Channel >= this.M_BandInfo.M_ChanInfo.M_UDlChanMin && Channel <= this.M_BandInfo.M_ChanInfo.M_UDlChanMax)
                    return true;
                else 
                    return false;
            }
            else
            {
                if (Channel >= this.M_BandInfo.M_ChanInfo.M_UUlChanMin && Channel <= this.M_BandInfo.M_ChanInfo.M_UUlChanMax)
                    return true;
                else
                    return false;
            }
        }
        public bool IsFreqIn(float Freq, bool Mode)//检查输入频率是否在频带内
        {
            //输入的Mode  true 代表DL， false 代表UL
            //返回的true代表在频带内，false表示不在频带内
            if (Mode)
            {
                //DL
                if (Freq >= this.M_BandInfo.M_Frequency.M_FDlmin && Freq <= this.M_BandInfo.M_Frequency.M_FDlmax)
                    return true;
                else
                    return false;
            }
            else
            {
                if (Freq >= this.M_BandInfo.M_Frequency.M_FUlmin && Freq <= this.M_BandInfo.M_Frequency.M_FUlmax)
                    return true;
                else
                    return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }



    /// <summary>
    /// Delegatecommand，这种WPF.SL都可以用，VIEW里面直接使用INTERACTION的trigger激发。比较靠谱，适合不同的UIElement控件
    /// </summary>
    public class CDelegateCommand : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> executeAction;
        bool canExecuteCache;

        public CDelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            bool temp = canExecute(parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }

            return canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }

        #endregion
    }
}
