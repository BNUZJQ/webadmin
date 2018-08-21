using Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FCCT.ViewModel;


namespace FCCT.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CMainWindow : Window
    {
        private enum RAT { NR, LTE, WCDMA, CDMA, TDSCDMA, GSM };
        //public CRATSupportBandsViewModel g_ratSupportBandsViewModel;
        //public CBandInfoViewModel g_bandInfoViewModel;
        public CMainViewModel MainView;
        public List<CheckBox> NRCheckBoxList,LTECheckBoxList,WCDMACheckBoxList,CDMACheckBoxList,TDSCDMACheckBoxList,GSMCheckBoxList;
        public List<RadioButton> SCSRadioButtonList, PowerClassRadioButtonList;
        public Line UlToDl = new Line();

        public CMainWindow()
        {
            MainView = new CMainViewModel();
            this.DataContext = MainView;
            InitializeComponent();
            InitFixedInfo();
            InitScrollbar();
            InitBandDisplay();
            InitAllowedBW();
            InitUEPower();
        }
        //初始化函数
        private void InitFixedInfo()//初始化界面静态元素信息绑定
        {
            Binding binding = new Binding();
            binding = new Binding();
            binding.Source = MainView;
            binding.Path = new PropertyPath("M_RATView.M_strWindowTitle");
            binding.Mode = BindingMode.OneWay;
            MainWindow.SetBinding(Window.TitleProperty, binding);

            //频段内频率、信道的最大值最小值中间值
            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strDLMinInfo");
            binding.Mode = BindingMode.OneWay;
            DLMinInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strDLMLInfo");
            binding.Mode = BindingMode.OneWay;
            DLMLInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strDLMHInfo");
            binding.Mode = BindingMode.OneWay;
            DLMHInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strDLMaxInfo");
            binding.Mode = BindingMode.OneWay;
            DLMaxInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strULMinInfo");
            binding.Mode = BindingMode.OneWay;
            ULMinInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strULMLInfo");
            binding.Mode = BindingMode.OneWay;
            ULMLInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strULMHInfo");
            binding.Mode = BindingMode.OneWay;
            ULMHInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strULMaxInfo");
            binding.Mode = BindingMode.OneWay;
            ULMaxInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_usChanSpacing");
            binding.Mode = BindingMode.OneWay;
            ChanSpacing.SetBinding(Label.ContentProperty, binding);

            //绑定当前信道的信道号、频率值
            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_strDisplayFreUl");
            binding.Mode = BindingMode.TwoWay;
            FreqUl.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_strDisplayFreDl");
            binding.Mode = BindingMode.TwoWay;
            FreqDl.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_UDisplayChanUl");
            binding.Mode = BindingMode.TwoWay;
            ChanUl.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_UDisplayChanDl");
            binding.Mode = BindingMode.TwoWay;
            ChanDl.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strSingleDlHidden");
            binding.Mode = BindingMode.OneWay;
            UlChanName.SetBinding(Label.VisibilityProperty, binding);
            ChanUl.SetBinding(Label.VisibilityProperty, binding);
            FreqUl.SetBinding(Label.VisibilityProperty, binding);
            UlName.SetBinding(Label.VisibilityProperty, binding);
            UlL.SetBinding(Label.VisibilityProperty, binding);
            UlML.SetBinding(Label.VisibilityProperty, binding);
            UlMH.SetBinding(Label.VisibilityProperty, binding);
            UlH.SetBinding(Label.VisibilityProperty, binding);
            ULMinInfo.SetBinding(Label.VisibilityProperty, binding);
            ULMLInfo.SetBinding(Label.VisibilityProperty, binding);
            ULMHInfo.SetBinding(Label.VisibilityProperty, binding);
            ULMaxInfo.SetBinding(Label.VisibilityProperty, binding);

            //在XML文件中把IsSingleUl添加进去
            
            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strSingleUlHidden");
            binding.Mode = BindingMode.OneWay;
            DlChanName.SetBinding(Label.VisibilityProperty, binding);
            ChanDl.SetBinding(Label.VisibilityProperty, binding);
            FreqDl.SetBinding(Label.VisibilityProperty, binding);
            DlName.SetBinding(Label.VisibilityProperty, binding);
            DlL.SetBinding(Label.VisibilityProperty, binding);
            DlML.SetBinding(Label.VisibilityProperty, binding);
            DlMH.SetBinding(Label.VisibilityProperty, binding);
            DlH.SetBinding(Label.VisibilityProperty, binding);
            DLMinInfo.SetBinding(Label.VisibilityProperty, binding);
            DLMLInfo.SetBinding(Label.VisibilityProperty, binding);
            DLMHInfo.SetBinding(Label.VisibilityProperty, binding);
            DLMaxInfo.SetBinding(Label.VisibilityProperty, binding);
            
        }
        private void InitScrollbar()//初始化滚动条绑定
        {
            //滚动条范围=信道数量
            Binding binding = new Binding();
            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_UScrollLength");
            binding.Mode = BindingMode.OneWay;
            ScrollBar.SetBinding(ScrollBar.MaximumProperty, binding);
            //滚动条当前值=M_DScrollBarValue
            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_DScrollBarValue");
            binding.Mode = BindingMode.TwoWay;
            ScrollBar.SetBinding(ScrollBar.ValueProperty, binding);

        }

        private void InitBandDisplay()        //初始化频带展示信息绑定
        {
            Binding binding = new Binding();
            //左连接线
            Line LeftLine = new Line();
            LeftLine.Stroke = System.Windows.Media.Brushes.Black;
            LeftLine.HorizontalAlignment = HorizontalAlignment.Left;
            LeftLine.VerticalAlignment = VerticalAlignment.Center;
            LeftLine.X2 = 80; LeftLine.Y2 = 45;
            LeftLine.Y1 = 30;
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_DLeftXPosition");
            binding.Mode = BindingMode.OneWay;
            LeftLine.SetBinding(Line.X1Property, binding);
            this.BandDisplayCanvas.Children.Add(LeftLine);
            //右连接线
            Line RightLine = new Line();
            RightLine.Stroke = System.Windows.Media.Brushes.Black;
            RightLine.HorizontalAlignment = HorizontalAlignment.Left;
            RightLine.VerticalAlignment = VerticalAlignment.Center;
            RightLine.X2 = 380; RightLine.Y2 = 45;
            RightLine.Y1 = 30;
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_DRightXPosition");
            binding.Mode = BindingMode.OneWay;
            RightLine.SetBinding(Line.X1Property, binding);
            this.BandDisplayCanvas.Children.Add(RightLine);
            //Dl端用绿线表示
            Line DlLine = new Line();
            DlLine.Stroke = System.Windows.Media.Brushes.Green;
            DlLine.HorizontalAlignment = HorizontalAlignment.Left;
            DlLine.VerticalAlignment = VerticalAlignment.Center;
            DlLine.StrokeThickness = 2;
            DlLine.Y2 = 60;
            DlLine.Y1 = 45;
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_CurFrePositionDl");
            binding.Mode = BindingMode.OneWay;
            DlLine.SetBinding(Line.X1Property, binding);
            DlLine.SetBinding(Line.X2Property, binding);
            this.BandDisplayCanvas.Children.Add(DlLine);
            //Ul端用蓝线表示
            Line UlLine = new Line();
            UlLine.Stroke = System.Windows.Media.Brushes.Blue;
            UlLine.HorizontalAlignment = HorizontalAlignment.Left;
            UlLine.VerticalAlignment = VerticalAlignment.Center;
            UlLine.StrokeThickness = 2;
            UlLine.Y2 = 60;
            UlLine.Y1 = 45;
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_CurFrePositionUl");
            binding.Mode = BindingMode.OneWay;
            UlLine.SetBinding(Line.X1Property, binding);
            UlLine.SetBinding(Line.X2Property, binding);
            this.BandDisplayCanvas.Children.Add(UlLine);
            //Ul到Dl的连接线
            UlToDl.Stroke = System.Windows.Media.Brushes.Gray;
            UlToDl.HorizontalAlignment = HorizontalAlignment.Left;
            UlToDl.VerticalAlignment = VerticalAlignment.Center;
            UlToDl.Y2 = 52.5;
            UlToDl.Y1 = 52.5;
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_CurFrePositionUl");
            binding.Mode = BindingMode.OneWay;
            UlToDl.SetBinding(Line.X1Property, binding);
            binding = new Binding();
            binding.Mode = BindingMode.OneWay;
            binding.Path = new PropertyPath("M_RATView.M_CurFrePositionDl");
            UlToDl.SetBinding(Line.X2Property, binding);
            this.BandDisplayCanvas.Children.Add(UlToDl);
            //BandBar内显示的文字信息绑定
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_strUlDlDifLabelContent");
            binding.Mode = BindingMode.OneWay;
            BandBarInfo.SetBinding(Label.ContentProperty, binding);

            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_ThickUlDlDifLabelMargin");
            binding.Mode = BindingMode.OneWay;
            BandBarInfo.SetBinding(Label.MarginProperty, binding);

            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_strBandBarInfo");
            binding.Mode = BindingMode.OneWay;
            BandBar.SetBinding(Label.ContentProperty, binding);
        }

        private void InitAllowedBW()//初始化允许频带信息绑定
        {
            //NR
            String[] strAllSCS_NR = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "NR").M_strSCS.Split(',');
            String[] strAllBandWidth_NR = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "NR").M_strBandWidth.Split(','); ;
            this.SCSRadioButtonList = new List<RadioButton>();
            foreach (String strSCS in strAllSCS_NR)//增加SCS的RadioButton,最大支持10个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllSCS_NR, strSCS);
                int j = i / 5;
                CreateSRSRadioButton_NR(strSCS, strSCS, "SCS", 40 + 50 * (i % 5), 25 + j * 20);
            }
            this.NRCheckBoxList = new List<CheckBox>();
            foreach (String strBandWidth in strAllBandWidth_NR)//增加BandWidth的CheckBox,最大支持20个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllBandWidth_NR, strBandWidth);
                int j = i / 5;
                CreateBandWidthCheckBox_WithSCS(strBandWidth, strBandWidth, 10 + 60 * (i % 5), 5 + 20 * j, NRCheckBoxList, Grid_NR_Custom);
            }
            SCSChange(null, null);
            //2个隐藏的RadioButton使得ViewModel层能够通过前端的命令刷新信息
            Binding binding = new Binding();
            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_curSCS");
            binding.Mode = BindingMode.TwoWay;
            binding.Converter = FindResource("stringToBoolConverter") as IValueConverter;
            binding.ConverterParameter = "Refresh";
            Refresh.SetBinding(RadioButton.IsCheckedProperty, binding);

            binding = new Binding();
            binding.Path = new PropertyPath("M_RATView.M_curSCS");
            binding.Mode = BindingMode.TwoWay;
            binding.Converter = FindResource("stringToBoolConverter") as IValueConverter;
            binding.ConverterParameter = "RefreshOther";
            RefreshOther.SetBinding(RadioButton.IsCheckedProperty, binding);

            //LTE
            String[] strAllBandWidth_LTE = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "LTE").M_strBandWidth.Split(',');
            this.LTECheckBoxList = new List<CheckBox>();
            foreach (String strBandWidth in strAllBandWidth_LTE)//增加BandWidth的CheckBox,最大支持20个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllBandWidth_LTE, strBandWidth);
                int j = i / 5;
                CreateBandWidthCheckBox_NOSCS(strBandWidth, strBandWidth, 10 + 60 * (i % 5), 25 + 20 * j, LTECheckBoxList, Grid_LTE_Custom);
            }

            //WCDMA
            String[] strAllBandWidth_WCDMA;
            if (MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "WCDMA").M_strBandWidth.Split(',') == null)
            {
                strAllBandWidth_WCDMA = new String[1]; 
                strAllBandWidth_WCDMA[0] = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "WCDMA").M_strBandWidth;
            }
            else
                strAllBandWidth_WCDMA = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "WCDMA").M_strBandWidth.Split(',');
            this.WCDMACheckBoxList = new List<CheckBox>();
            foreach (String strBandWidth in strAllBandWidth_WCDMA)//增加BandWidth的CheckBox,最大支持20个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllBandWidth_WCDMA, strBandWidth);
                int j = i / 5;
                CreateBandWidthCheckBox_NOSCS(strBandWidth, strBandWidth, 10 + 60 * (i % 5), 25 + 20 * j, WCDMACheckBoxList, Grid_WCDMA_Custom);
            }

            //CDMA
            String[] strAllBandWidth_CDMA;
            if (MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "CDMA").M_strBandWidth.Split(',') == null)
            {
                strAllBandWidth_CDMA = new String[1];
                strAllBandWidth_CDMA[0] = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "CDMA").M_strBandWidth;
            }
            else
                strAllBandWidth_CDMA = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "CDMA").M_strBandWidth.Split(',');
            this.CDMACheckBoxList = new List<CheckBox>();
            foreach (String strBandWidth in strAllBandWidth_CDMA)//增加BandWidth的CheckBox,最大支持20个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllBandWidth_CDMA, strBandWidth);
                int j = i / 5;
                CreateBandWidthCheckBox_NOSCS(strBandWidth, strBandWidth, 10 + 60 * (i % 5), 25 + 20 * j, CDMACheckBoxList, Grid_CDMA_Custom);
            }

            //GSM
            String[] strAllBandWidth_GSM;
            if (MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "GSM").M_strBandWidth.Split(',') == null)
            {
                strAllBandWidth_GSM = new String[1];
                strAllBandWidth_GSM[0] = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "GSM").M_strBandWidth;
            }
            else
                strAllBandWidth_GSM = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "GSM").M_strBandWidth.Split(',');
            this.GSMCheckBoxList = new List<CheckBox>();
            foreach (String strBandWidth in strAllBandWidth_GSM)//增加BandWidth的CheckBox,最大支持20个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllBandWidth_GSM, strBandWidth);
                int j = i / 5;
                CreateBandWidthCheckBox_NOSCS(strBandWidth, strBandWidth, 10 + 60 * (i % 5), 25 + 20 * j, GSMCheckBoxList, Grid_GSM_Custom);
            }
     
        }
        private void InitUEPower()
        {
            Binding binding;
            binding = new Binding();
            binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_BandInfoView.M_MaxPowerInfo");
            binding.Mode = BindingMode.OneWay;
            UEPowerList_NR.SetBinding(ListView.ItemsSourceProperty, binding);
            UEPowerList_LTE.SetBinding(ListView.ItemsSourceProperty, binding);
            UEPowerList_WCDMA.SetBinding(ListView.ItemsSourceProperty, binding);
            UEPowerList_CDMA.SetBinding(ListView.ItemsSourceProperty, binding);
            UEPowerList_TDSCDMA.SetBinding(ListView.ItemsSourceProperty, binding);

            this.PowerClassRadioButtonList = new List<RadioButton>();
            //GSM协议需要选择PowerClass，这里只初始化表格以及选择按钮，处理逻辑放在PowerClassChange事件中
            String[] strAllPowerClass_GSM = MainView.M_RATSet.M_strFormatList.Find(s => s.M_strRAT == "GSM").M_strPowerClass.Split(',');
            foreach (String strPowerClass in strAllPowerClass_GSM)//增加SCS的RadioButton,最大支持16个SCS,否则需要更改前端布局和这里的代码
            {
                int i = Array.IndexOf(strAllPowerClass_GSM, strPowerClass);
                int j = i / 8;
                CreatePowerClassRadioButton_GSM(strPowerClass, strPowerClass, "PowerClass", 10 + 40 * (i % 8), 25 + j * 20);
            }
        }

        //创造一个SCS选择的Button，只适用NR协议
        private void CreateSRSRadioButton_NR(String RBName,String RBContent,String GroupName,int RBLeft,int RBTop)
        {
            RadioButton RB = new RadioButton();
            RB.Name ="SCS" + RBName + "K";
            RB.Content = RBContent + "K";
            RB.Height = 16; RB.Width = 50; RB.FontSize = 10; RB.GroupName = GroupName; 
            RB.HorizontalAlignment = HorizontalAlignment.Left; RB.VerticalAlignment = VerticalAlignment.Top;
            RB.Margin = new Thickness(RBLeft, RBTop, 0, 0);
            Grid_NR_Custom.Children.Add(RB);
            RB.SetValue(Grid.RowProperty, 0);
            RB.Checked += SCSChange;
            Style radBase = (Style)this.FindResource("radBase");//TabItemStyle 这个样式是引用的资源文件中的样式名称
            RB.Style = radBase;

            Binding binding = new Binding();
            binding = new Binding();
            //binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_RATView.M_curSCS");
            binding.Mode = BindingMode.TwoWay;
            binding.Converter = FindResource("stringToBoolConverter") as IValueConverter;
            binding.ConverterParameter = RBContent;
            RB.SetBinding(RadioButton.IsCheckedProperty, binding);
            SCSRadioButtonList.Add(RB);
        }
        //创建一个允许带宽的CheckBox，适用于可选SCS的协议
        private void CreateBandWidthCheckBox_WithSCS(String CBName, String CBContent, int CBLeft, int CBTop, List<CheckBox> CheckBoxList,Grid grid)
        {
            CheckBox CB = new CheckBox();
            CB.Name = "BW" + CBName + "M";
            CB.Content = CBContent + "M";
            CB.Height = 16; CB.Width = 60; CB.FontSize = 12; CB.IsEnabled = false;
            CB.HorizontalAlignment = HorizontalAlignment.Left; CB.VerticalAlignment = VerticalAlignment.Top;
            CB.Margin = new Thickness(CBLeft, CBTop, 0, 0);
            grid.Children.Add(CB);
            CB.SetValue(Grid.RowProperty, 1);
            Style ChkBase = (Style)this.FindResource("chkBase");//TabItemStyle 这个样式是引用的资源文件中的样式名称
            CB.Style = ChkBase;
            CheckBoxList.Add(CB);
        }
        //创建一个允许带宽的CheckBox，适用于SCS固定的协议
        private void CreateBandWidthCheckBox_NOSCS(String CBName, String CBContent, int CBLeft, int CBTop, List<CheckBox> CheckBoxList,Grid grid)
        {
            CheckBox CB = new CheckBox();
            //CB.Name = "BW" + CBName + "M";
            CB.Content = CBContent + "M";
            CB.Height = 16; CB.Width = 60; CB.FontSize = 12; CB.IsEnabled = false;
            CB.HorizontalAlignment = HorizontalAlignment.Left; CB.VerticalAlignment = VerticalAlignment.Top;
            CB.Margin = new Thickness(CBLeft, CBTop, 0, 0);
            grid.Children.Add(CB);
            CB.SetValue(Grid.RowProperty, 0);
            Style ChkBase = (Style)this.FindResource("chkBase");//TabItemStyle 这个样式是引用的资源文件中的样式名称
            CB.Style = ChkBase;
            CheckBoxList.Add(CB);
        }

        //创建一个PowerClass的选择Button——只适用GSM协议
        private void CreatePowerClassRadioButton_GSM(String RBName, String RBContent, String GroupName, int RBLeft, int RBTop)
        {
            RadioButton RB = new RadioButton();
            RB.Name = "PowerClass" + RBName;
            RB.Content = RBContent;
            RB.Height = 16; RB.Width = 40; RB.FontSize = 10; RB.GroupName = GroupName;
            RB.HorizontalAlignment = HorizontalAlignment.Left; RB.VerticalAlignment = VerticalAlignment.Top;
            RB.Margin = new Thickness(RBLeft, RBTop, 0, 0);
            Grid_GSM_Custom.Children.Add(RB);
            RB.SetValue(Grid.RowProperty, 1);
            RB.Checked += PowerClassChange;
            Style radBase = (Style)this.FindResource("radBase");//TabItemStyle 这个样式是引用的资源文件中的样式名称
            RB.Style = radBase;

            Binding binding = new Binding();
            binding = new Binding();
            //binding.Source = MainView.M_RATView;
            binding.Path = new PropertyPath("M_RATView.M_curPowerClass");
            binding.Mode = BindingMode.TwoWay;
            binding.Converter = FindResource("stringToBoolConverter") as IValueConverter;
            binding.ConverterParameter = RBContent;
            RB.SetBinding(RadioButton.IsCheckedProperty, binding);

            PowerClassRadioButtonList.Add(RB);
        }
        private void ChanDlKeyDown(object sender, KeyEventArgs e)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;//判断shifu键是否按下
            if (shiftKey == true)                  //当按下shift
            {
                e.Handled = true;//不可输入
            }
            else//未按shift
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter))
                {
                    e.Handled = true;//不可输入
                }
                if (e.Key == Key.Enter)
                {
                    ChanDl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        private void ChanDlLostFocus(object sender, RoutedEventArgs e)
        {
            ChanDl.Text = ChanDl.Text.Replace(" ", "");
            if (ChanDl.Text == "")
                ChanDl.Text = "0";
        }

        private void ChanUlKeyDown(object sender, KeyEventArgs e)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;//判断shifu键是否按下
            if (shiftKey == true)                  //当按下shift
            {
                e.Handled = true;//不可输入
            }
            else//未按shift
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter))
                {
                    e.Handled = true;//不可输入
                }
                if (e.Key == Key.Enter)
                {
                    ChanDl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                }
            }
        }

        private void ChanUlLostFocus(object sender, RoutedEventArgs e)
        {
            ChanUl.Text = ChanUl.Text.Replace(" ", "");
            if (ChanUl.Text == "")
                ChanUl.Text = "0";
        }

        private void FreqUlKeyDown(object sender, KeyEventArgs e)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;//判断shifu键是否按下
            if (shiftKey == true)                  //当按下shift
            {
                e.Handled = true;//不可输入
            }
            else//未按shift
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Decimal || e.Key == Key.OemPeriod))
                {
                    e.Handled = true;//不可输入
                }
                if (e.Key == Key.Enter)
                {
                    FreqDl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                }
            }
        }

        private void FreqUlLostFocus(object sender, RoutedEventArgs e)
        {
            FreqDl.Text = FreqDl.Text.Replace(" ", "");
            if (FreqDl.Text == "")
                FreqDl.Text = "0";
            
        }

        private void FreqDlKeyDown(object sender, KeyEventArgs e)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;//判断shifu键是否按下
            if (shiftKey == true)                  //当按下shift
            {
                e.Handled = true;//不可输入
            }
            else//未按shift
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Decimal || e.Key == Key.OemPeriod))
                {
                    e.Handled = true;//不可输入
                }
                if (e.Key == Key.Enter)
                {
                    FreqDl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        private void FreqDlLostFocus(object sender, RoutedEventArgs e)
        {
            FreqUl.Text = FreqUl.Text.Replace(" ", "");
            if (FreqUl.Text == "")
                FreqUl.Text = "0";
        }
        private void PowerClassChange(object sender, RoutedEventArgs e)
        {
            UEPowerList_GSM.Items.Clear();
            String curPowerClass = MainView.M_RATView.M_curPowerClass;
            if (PowerClassRadioButtonList == null)
                goto Lable;
            foreach (RadioButton RB in PowerClassRadioButtonList)
            {
                RB.IsEnabled = false;
                //存在一条Item，使得Item中的M_strClass字段与当前RadioButton的Content匹配，则使其使能
                if (MainView.M_RATView.M_curBandInfo.M_PowerInfo.M_PowerItem.Exists(s => s.M_strClass == RB.Content.ToString()))
                    RB.IsEnabled = true;
            }
Lable:
            if (MainView.M_RATView.M_BandInfoView.M_MaxPowerInfo.Find(s => s.M_strClass == curPowerClass) == null)
            {
                //如果没有信息，就不再往下走，清空列表然后返回
                return;
            }
            float tempMaxPower = MainView.M_RATView.M_BandInfoView.M_MaxPowerInfo.Find(s => s.M_strClass == curPowerClass).M_FMaxPower;
            foreach (CPowerTable item in MainView.M_RATView.M_BandInfoView.M_PowerCtrLev)
            {
                //如果功率比频带规定的最大功率小，那就把这一条显示在表格中
                if (item.M_FOutput <= tempMaxPower)
                    UEPowerList_GSM.Items.Add(item);
            }
            //防止XML中的表格没有按顺序列出，自动排序
            //UEPowerList_GSM.Items.SortDescriptions.Add(new SortDescription("Output", ListSortDirection.Descending));
        }

        private void SCSChange(object sender, RoutedEventArgs e)
        {
            if (this.MainView.M_RATView.M_curBandInfo.M_HisiInfo.M_strIsSingleDL == "True" || this.MainView.M_RATView.M_curBandInfo.M_HisiInfo.M_strIsSingleUL == "True" || this.MainView.M_RATView.M_curBandInfo.M_HisiInfo.M_strDuplexMode == "TDD")
            { 
                UlToDl.Visibility = Visibility.Hidden;
                BandBarInfo.Visibility = Visibility.Hidden;
            }
            else
            {
                UlToDl.Visibility = Visibility.Visible;
                BandBarInfo.Visibility = Visibility.Visible;
            }


            List<CheckBox> tempCheckBoxList = new List<CheckBox>();
            switch (MainView.M_RATView.M_RAT.M_strRAT)//判断当前制式
            {
                case "NR":
                    {
                        tempCheckBoxList = NRCheckBoxList;
                        //刷新SCS的信息，把该频段不支持的SCS disable
                        if (SCSRadioButtonList == null)
                            break;
                        foreach (RadioButton RB in SCSRadioButtonList)
                        {
                            RB.IsEnabled = false;
                            if (MainView.M_RATView.M_curBandInfo.M_BandWidth.Exists(s => (s.M_strSCS + "K") == RB.Content.ToString()))
                                RB.IsEnabled = true;
                        }
                        break;
                    }
                case "LTE":
                    {
                        tempCheckBoxList = LTECheckBoxList;
                        break;
                    }
                case "WCDMA":
                    {
                        tempCheckBoxList = WCDMACheckBoxList;
                        break;
                    }
                case "CDMA":
                    {
                        tempCheckBoxList = CDMACheckBoxList;
                        break;
                    }
                case "TDSCDMA":
                    {
                        tempCheckBoxList = TDSCDMACheckBoxList;
                        break;
                    }
                case "GSM":
                    {
                        tempCheckBoxList = GSMCheckBoxList;
                        PowerClassChange(null, null);//同时触发PowerClassChange改变的命令
                        break;
                    }
            }
            if (tempCheckBoxList == null)
                return;
            String[] AllowedBW = MainView.M_RATView.M_AllowedBW;
            foreach (CheckBox tempCB in tempCheckBoxList)
            {
                tempCB.IsChecked = false;
                if (AllowedBW == null)
                    continue;
                foreach (String eachBW in AllowedBW)
                {
                    if (tempCB.Content.ToString() == (eachBW + "M").ToString())
                        tempCB.IsChecked = true;
                }
            }
        }
    }
    public class StringToBoolConverter : IValueConverter
    {
        //数据转换器，实现String格式到布尔格式的转换，用于RadioButton值的绑定
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            String checkvalue = value.ToString();
            String targetvalue = parameter.ToString();
            return checkvalue.Equals(targetvalue, StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            bool isChecked = (bool)value;
            if (!isChecked)
            {
                return null;
            }
            return parameter.ToString();
        }
    }
}
