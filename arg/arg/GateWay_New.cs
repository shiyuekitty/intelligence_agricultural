using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arg
{
    class GateWay_New
    {
        #region

        private WSN_New_DLL.Gateway gateway;
        public bool IsConnect = false;

        public string JiDianQi1 = "2001";
        public string JiDianQi2 = "2003";
        public string TianChuang = "00";
        public string ShuiLian = "00";
        public string PenGuan = "00";
        public string WaiZheYang = "00";
        public string NeiZheYang = "00";
        public string WaiTongFeng = "00";
        public string NeiTongFeng = "00";

        public string AirTemp = "";
        public string AirHumi = "";
        public string SoilTemp = "";
        public string SoilHumi = "";
        public string Light = "";
        public string CO2 = "";
        public string IP = "";
        public string Port = "";

        #endregion

        #region
        public GateWay_New()
        {
        }
        #endregion

        #region
        public bool Connect()
        {
            try
            {
                gateway = new WSN_New_DLL.Gateway(this.IP, Convert.ToInt32(this.Port));
                gateway.EventDataArrival += gateway_EventDataArrival;
                if (gateway.Connect())
                {
                    return IsConnect = true;
                }
                else
                {
                    return IsConnect = false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string Ping()
        {
            if (IsConnect)
            {
                if (gateway.SendData(new string[] { "0000", "FFFFFFFFFF" }))
                {
                    return "";
                }
                else
                {
                    return "[Ping]错误!";
                }
            }
            return "[Ping]未连接网关!";
        }

        public string ReadNodeData(string address)
        {
            if (IsConnect)
            {
                if (gateway.SendData(new string[] { address, "FFFFFFFFFF" }))
                {
                    return "";
                }
                else
                {
                    return "[ReadNodeData]错误!";
                }
            }
            return "[ReadNodeData]未连接网关!";
        }

        public string SendNodeData(string[] command)
        {
            if (IsConnect)
            {
                if (gateway.SendData(command))
                {
                    return "";
                }
                else
                {
                    return "[SendNodeData]错误!";
                }
            }
            return "[SendNodeData]未连接网关!";
        }

        public string ConnectTest()
        {
            if (this.IsConnect)
            {
                return "连接网关成功";
            }
            else
            {
                return "连接网关失败";
            }
        }
        void gateway_EventDataArrival(List<string[]> data)
        {
            foreach (string[] s in data)
            {
                switch (s[0].Substring(0, 2))
                {
                    case "31":
                        AirTemp = (((double)Convert.ToInt32(s[1].Substring(0, 4), 16)) / 100).ToString();
                        AirHumi = (((double)Convert.ToInt32(s[1].Substring(4, 4), 16)) / 100).ToString();
                        break;
                    case "32":
                        SoilTemp = (((double)Convert.ToInt32(s[1].Substring(0, 4), 16)) / 100).ToString();
                        SoilHumi = (((double)Convert.ToInt32(s[1].Substring(4, 4), 16)) / 100).ToString();
                        break;
                    case "34":
                        Light = Convert.ToInt32(s[1].Substring(0, 4), 16).ToString();
                        break;
                    case "33":
                        CO2 = Convert.ToInt32(s[1].Substring(0, 4), 16).ToString();
                        break;
                    case "20":
                        if (s[0] == JiDianQi1)
                        {
                            WaiZheYang = s[1].Substring(0, 2);
                            NeiZheYang = s[1].Substring(2, 2);
                            WaiTongFeng = s[1].Substring(4, 2);
                            NeiTongFeng = s[1].Substring(6, 2);
                        }
                        if (s[0] == JiDianQi2)
                        {
                            TianChuang = s[1].Substring(0, 2);
                            PenGuan = s[1].Substring(2, 2);
                            ShuiLian = s[1].Substring(4, 2);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region

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

        #endregion


        #region  记录继电器设备状态 节能

        private bool statusNTF;//内通风 ture 为开 flase 为关
        private bool statusWTF;//外通风
        private bool statusWZY;//外遮阳
        private bool statusNZY;//内遮阳
        private bool statusTC;//天窗
        private bool statusPG;//喷灌
        private bool statusSL;//水帘

        private bool smartValueStatus; ///智能控制值设置状态先设置阈值才能控制
        private bool isSmartControl = false;  ///是否开启智能控制

        #endregion
    }

}


