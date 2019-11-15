using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplicationExamAutoSubmit
{
    class Program
    {
        private static string urlGet = "http://wechat.sctcn.com/SctWeChat_Exam/WX/GetExamList?ExamTypeNo&Pname=赵子清&Pno=SCT01975&Pdepart=信息技术部&Type=WX";
        private static string urlPost = "http://wechat.sctcn.com/SctWeChat_Exam/WX/SubmitExam";
        static void Main(string[] args)
        {
            string resGet = HttpHelper.Get(urlGet);
            Console.WriteLine(resGet);
            List<ExamModel> objResGet = JsonConvert.DeserializeObject<List<ExamModel>>(resGet);

            foreach(ExamModel objEle in objResGet)
            {
                objEle.EXAMNOTE = string.IsNullOrEmpty(objEle.EANSWERX) ? objEle.EANSWERP : objEle.EANSWERX;                
            }

            string resAnswerGet = JsonConvert.SerializeObject(objResGet);

            string resPost  = HttpHelper.Post(urlPost, resAnswerGet, "application/json", Encoding.UTF8);

            Console.WriteLine("分数：" + resPost);

            Console.ReadKey();
        }
    }
}
