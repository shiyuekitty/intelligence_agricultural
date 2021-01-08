using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arg
{
    public partial class Form1 : Form
    {
        Thread thread1;
        Thread thread;
        private GateWay_New listGateWay;
        private bool statusNTF;//内通风 ture 为开 flase 为关
        private bool statusWTF;//外通风
        private bool statusWZY;//外遮阳
        private bool statusNZY;//内遮阳
        private bool statusTC;//天窗
        private bool statusPG;//喷灌
        private bool statusSL;//水帘

        private bool State = false;
        private bool smartValueStatus; ///智能控制值设置状态先设置阈值才能控制
        private bool isSmartControl = false;  ///是否开启智能控制
        public Form1()
        {
            InitializeComponent();
            Form1.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                listGateWay = new GateWay_New();
                listGateWay.IP = txtGatewayIP.Text;
                listGateWay.Port = txtGatewayPort.Text;
                if (listGateWay.Connect())
                {
                    tishi1.Text = "连接网关成功!";
                    tishi1.Visible = true;
                    thread1 = new Thread(Getdata);
                    thread1.IsBackground = true;
                    thread1.Start();
                    Thread.Sleep(100);
                    timer1.Start();
                }
                else
                {
                    tishi1.Text = "连接网关失败！请检查连接";
                    tishi1.Visible = true;
                }
            }
            catch
            {
            }
        }

        public void Getdata(object obj)
        {
            while(true)
            {
                labTemp.Text=listGateWay.AirTemp;
                labHumi.Text=listGateWay.AirHumi;
                labSolidTemp.Text=listGateWay.SoilTemp;
                labSolidHumi.Text=listGateWay.SoilHumi;
                labLight.Text=listGateWay.Light;
                labCO2.Text=listGateWay.CO2;
                Thread.Sleep(100);
            }
        }

        public void GetState()
        {
            Thread.Sleep(500);
            if (listGateWay.WaiZheYang == "01")
            {
                btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("外遮阳开");
                btnWZY.Tag = 1;
                statusWZY = true;
            }
            else if (listGateWay.WaiZheYang == "00")
            {
                btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("外遮阳关");
                btnWZY.Tag = 2;
                statusWZY = false;
            }

            if (listGateWay.NeiZheYang == "01")
            {
                btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("内遮阳开");
                btnNZY.Tag = 1;
                statusNZY = true;
            }
            else if (listGateWay.NeiZheYang == "00")
            {
                btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("内遮阳关");
                btnNZY.Tag = 2;
                statusNZY = false;
            }

            if (listGateWay.WaiTongFeng == "01")
            {
                btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picWTF.Enabled = true;
                btnWTF.Tag = 1;
                statusWTF = true;
            }
            else if (listGateWay.WaiTongFeng == "00")
            {
                btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picWTF.Enabled = false;
                btnWTF.Tag = 2;
                statusWTF = false;
            }

            if (listGateWay.NeiTongFeng == "01")
            {
                btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picNTF.Enabled = true;
                btnNTF.Tag = 1;
                statusNTF = true;
            }
            else if (listGateWay.NeiTongFeng == "00")
            {
                btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picNTF.Enabled = false;
                btnNTF.Tag = 2;
                statusNTF = false;
            }

            if (listGateWay.TianChuang == "01")
            {
                btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("天窗开");
                btnTC.Tag = 1;
                statusTC = true;
            }
            else if (listGateWay.TianChuang == "00")
            {
                btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("天窗关");
                btnTC.Tag = 2;
                statusTC = false;
            }

            if (listGateWay.PenGuan == "01")
            {
                btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picPG.Enabled = true;
                btnPG.Tag = 1;
                statusPG = true;
            }
            else if (listGateWay.PenGuan == "00")
            {
                btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picPG.Enabled = false;
                btnPG.Tag = 2;
                statusPG = false;
            }

            if (listGateWay.ShuiLian == "01")
            {
                btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                picSL.Enabled = true;
                btnSL.Tag = 1;
                statusSL = true;
            }
            else if (listGateWay.ShuiLian == "00")
            {
                btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                picSL.Enabled = false;
                btnSL.Tag = 2;
                statusSL = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listGateWay.IsConnect)
            {
                listGateWay.Ping();
                GetState();
            }
        }

        private void SendData(int i)
        {
            Thread.Sleep(100);
            if (listGateWay.IsConnect)
            {
                string[] str = new string[2];
                if (i == 1)
                {
                    str[0] = listGateWay.JiDianQi1;
                    str[1] = listGateWay.WaiZheYang + listGateWay.NeiZheYang + listGateWay.WaiTongFeng + listGateWay.NeiTongFeng + "FF";
                }
                else
                {
                    str[0] = listGateWay.JiDianQi2;
                    str[1] = listGateWay.TianChuang + listGateWay.PenGuan + listGateWay.ShuiLian + "00FF";
                }
                listGateWay.SendNodeData(str);
                Thread.Sleep(100);
            }
        }

        private void btnNZY_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnNZY.Tag.ToString() == "2")
                {
                    listGateWay.NeiZheYang = "01";
                    btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("内遮阳开");
                    btnNZY.Tag = 1;
                    statusNZY = true;
                }
                else
                {
                    listGateWay.NeiZheYang = "00";
                    btnNZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picNZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("内遮阳关");
                    btnNZY.Tag = 2;
                    statusNZY = false;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnWZY_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnWZY.Tag.ToString() == "2")
                {
                    listGateWay.WaiZheYang = "01";
                    btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("外遮阳开");
                    btnWZY.Tag = 1;
                    statusWTF = true;
                }
                else
                {
                    listGateWay.WaiZheYang = "00";
                    btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("外遮阳关");
                    btnWZY.Tag = 2;
                    statusWTF = false;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnNTF_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnNTF.Tag.ToString() == "2")
                {
                    listGateWay.NeiTongFeng = "01";
                    btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picNTF.Enabled = true;
                    btnNTF.Tag = 1;
                    statusNTF = true;
                }
                else
                {
                    listGateWay.NeiTongFeng = "00";
                    btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picNTF.Enabled = false;
                    btnNTF.Tag = 2;
                    statusNTF = false;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnWTF_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnWTF.Tag.ToString() == "2")
                {
                    listGateWay.WaiTongFeng = "01";
                    btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picWTF.Enabled = true;
                    btnWTF.Tag = 1;
                    statusWTF = true;
                }
                else
                {
                    listGateWay.WaiTongFeng = "00";
                    btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picWTF.Enabled = false;
                    btnWTF.Tag = 2;
                    statusWTF = false;
                }
                SendData(1);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnTC_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnTC.Tag.ToString() == "2")
                {
                    listGateWay.TianChuang = "01";
                    btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("天窗开");
                    btnTC.Tag = 1;
                    statusTC = true;
                }
                else
                {
                    listGateWay.TianChuang = "00";
                    btnTC.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("天窗关");
                    btnTC.Tag = 2;
                    statusTC = false;
                }
                SendData(2);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnPG_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnPG.Tag.ToString() == "2")
                {
                    listGateWay.PenGuan = "01";
                    btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picPG.Enabled = true;
                    btnPG.Tag = 1;
                    statusPG = true;
                }
                else
                {
                    listGateWay.PenGuan = "00";
                    btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picPG.Enabled = false;
                    btnPG.Tag = 2;
                    statusPG = false;
                }
                SendData(2);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnSL_Click(object sender, EventArgs e)
        {
            if (!isSmartControl)
            {
                if (btnSL.Tag.ToString() == "2")
                {
                    listGateWay.ShuiLian = "01";
                    btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    picSL.Enabled = true;
                    btnSL.Tag = 1;
                    statusSL = true;
                }
                else
                {
                    listGateWay.ShuiLian = "00";
                    btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    picSL.Enabled = false;
                    btnSL.Tag = 2;
                    statusSL = false;
                }
                SendData(2);
            }
            else
            {
                MessageBox.Show("当前为智能控制模式！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }


        //智能控制
        double MaxAirTemp = 35;//最大空气温度
        double MinAirTemp = 15; //最小空气温度
        double MaxAirHumi = 50;//最大空气湿度
        double MinAirHumi = 20;//最小空气湿度
        double MaxSoilTemp = 25;//最大土壤温度
        double MinSoilTemp = 15; //最小土壤温度
        double MaxSoilHumi = 44; //最大土壤湿度
        double MinSoilHumi = 17; //最小土壤湿度
        double MaxIlluminance = 2500; //最大光照度
        double MinIlluminance = 200; ///最小光照度
        double MaxC02Thickness = 700; //最大二氧化碳浓度
        double MinC02Thickness = 365; //最小二氧化碳浓度

        ///<summary>
        ///获取默认值
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void btnGetDefault_Click_1(object sender, EventArgs e)
        {
            txtHMin.Text = MinAirHumi.ToString();
            txtHMax.Text = MaxAirHumi.ToString();
            txtTMin.Text = MinAirTemp.ToString();
            txtTMax.Text = MaxAirTemp.ToString();
            txtSHMax.Text = MaxSoilHumi.ToString();
            txtSHMin.Text = MinSoilHumi.ToString();
            txtSTMax.Text = MaxSoilTemp.ToString();
            txtSTMin.Text = MinSoilTemp.ToString();
            txtLuxMax.Text = MaxIlluminance.ToString();
            txtLuxMin.Text = MinIlluminance.ToString();
            txtPPMMax.Text = MaxC02Thickness.ToString();
            txtPPMMin.Text = MinC02Thickness.ToString();
        }

        private void btnkeep_Click(object sender, EventArgs e)
        {
            try
            {
                MinAirHumi = Convert.ToDouble(txtHMin.Text);
                MaxAirHumi = Convert.ToDouble(txtHMax.Text);
                MinAirTemp = Convert.ToDouble(txtTMin.Text);
                MaxAirTemp = Convert.ToDouble(txtTMax.Text);
                MaxSoilHumi = Convert.ToDouble(txtSHMax.Text);
                MinSoilHumi = Convert.ToDouble(txtSHMin.Text);
                MaxSoilTemp = Convert.ToDouble(txtSTMax.Text);
                MinSoilTemp = Convert.ToDouble(txtSTMin.Text);
                MaxC02Thickness = Convert.ToDouble(txtPPMMax.Text);
                MinC02Thickness = Convert.ToDouble(txtPPMMin.Text);
                MaxIlluminance = Convert.ToDouble(txtLuxMax.Text);
                MinIlluminance = Convert.ToDouble(txtLuxMin.Text);
                smartValueStatus = true;//设置阈值标志变量值为真
                MessageBox.Show("保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("数值输入错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSmartSet_Click(object sender, EventArgs e)
        {
            if (smartValueStatus)
            {
                if (btnSmartSet.Tag.ToString()=="2")
                {
                    btnSmartSet.Tag = 1;
                    btnSmartSet.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    State = true;
                    thread = new Thread(SmartSet);
                    thread.Start();
                    isSmartControl = true;
                }
                else
                {
                    btnSmartSet.Tag = 2;
                    btnSmartSet.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    State = false;
                    thread.Abort();
                    isSmartControl = false;
                }
            }
            else
            {
                MessageBox.Show("请设置智能控制阀值！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //线程控制函数
        private void SmartSet(object obj)
        {
            while (State)
            {
                //天窗
                if (Convert.ToDouble(listGateWay.AirTemp)>MaxAirTemp)
                {
                    if (!statusTC)
                    {
                        statusTC = true;
                        listGateWay.TianChuang = "01";
                        picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("天窗开");
                        btnTC.Tag = 1;
                        picTC.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }
                }
                else if (Convert.ToDouble(listGateWay.AirTemp) < MinAirTemp)
                {
                    if (statusTC)
                    {
                        statusTC = false;
                        listGateWay.TianChuang = "00";
                        picTC.Image= (Bitmap)Properties.Resources.ResourceManager.GetObject("天窗关");
                        btnTC.Tag = 2;
                        btnTC.BackgroundImage= (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
                }


                //水帘
                if (Convert.ToDouble(listGateWay.AirHumi) < MinAirHumi)
                {
                    if (!statusSL)
                    {
                        statusSL = true;
                        listGateWay.ShuiLian = "01";
                        picSL.Enabled = true;
                        picSL.Tag = 1;
                        btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }
                }
                else if (Convert.ToDouble(listGateWay.AirHumi) >MaxAirHumi)
                {
                    if (statusSL)
                    {
                        statusSL = false;
                        listGateWay.ShuiLian = "00";
                        picSL.Enabled = false;
                        picSL.Tag = 2;
                        btnSL.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
                }
                   
                
                //喷灌
                if (Convert.ToDouble(listGateWay.SoilHumi) < MinSoilHumi)
                {
                    if (!statusPG)
                    {
                        statusPG = true;
                        listGateWay.PenGuan = "01";
                        picPG.Enabled = true;
                        picPG.Tag = 1;
                        btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }
                }
                else if (Convert.ToDouble(listGateWay.SoilHumi) > MaxSoilHumi)
                {
                    if (statusPG)
                    {
                        statusPG = false;
                        listGateWay.PenGuan = "00";
                        picPG.Enabled = false;
                        picPG.Tag = 2;
                        btnPG.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
                }
                
                //内通风
                
                if (Convert.ToDouble(listGateWay.SoilTemp) > MaxSoilTemp)
                {
                    if (!statusNTF)
                    {
                        statusNTF = true;
                        listGateWay.NeiTongFeng = "01";
                        picNTF.Enabled = true;
                        picNTF.Tag = 1;
                        btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }

                }
                else if (Convert.ToDouble(listGateWay.SoilTemp) < MinSoilTemp)
                {
                    if (statusNTF)
                    {
                        statusNTF = false;
                        listGateWay.NeiTongFeng = "00";
                        picNTF.Enabled = false;
                        picNTF.Tag = 2;
                        btnNTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
                }
                
                //外通风
                
                if (Convert.ToDouble(listGateWay.CO2) > MaxC02Thickness)
                {
                    if (!statusWTF)
                    {
                        statusWTF = true;
                        listGateWay.WaiTongFeng = "01";
                        picWTF.Enabled = true;
                        picWTF.Tag = 1;
                        btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }
                }
                else if (Convert.ToDouble(listGateWay.CO2) < MinC02Thickness)
                {
                    if (statusWTF)
                    {
                        statusWTF = false;
                        listGateWay.WaiTongFeng = "00";
                        picWTF.Enabled = false;
                        picWTF.Tag = 2;
                        btnWTF.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
                }
                
                //遮阳

                if (Convert.ToDouble(listGateWay.Light) < MinIlluminance)
                {
                    if (!statusWZY)
                    {
                        statusWZY = true;
                        listGateWay.WaiZheYang = "01";
                        picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("外遮阳开");
                        //picWZY.Enabled = true;
                        picWZY.Tag = 1;
                        btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("on");
                    }    
                }
                else if (Convert.ToDouble(listGateWay.Light) > MaxIlluminance)
                {
                    if (statusWZY)
                    {
                        statusWZY = false;
                        listGateWay.WaiZheYang = "00";
                        picWZY.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("外遮阳关");
                        //picWZY.Enabled = true;
                        picWZY.Tag = 2;
                        btnWZY.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("off");
                    }
                }
                SendData(1);
                SendData(2);
                Thread.Sleep(100);
             }
         }
     }
}

