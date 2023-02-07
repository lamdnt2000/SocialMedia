using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Schedule
{
    public class TriggerUtil
    {

        public static string JAR_CMD = "java -jar";
        public static string PARSER = "Parser.jar";
        public static string TOKEN = "Token.jar";
 
        public static string CreateRequest(string platform, string user, string handler)
        {
            var result = "";
           
            var command = JAR_CMD + " " + handler + " " + platform + " \"" + user +"\"";
            using (var client = new SshClient("ip", "user", "pass"))
            {

                client.Connect();
                var infor = client.ConnectionInfo;
                var sshCommand = client.CreateCommand(command);
                sshCommand.Execute();
                result = sshCommand.Result;
                client.Disconnect();
            }
            return result;
        }
       
        public static string UpdateRequest(string platform, string user, string handler, int id)
        {
            var result = "";
            //user = user.StartsWith("@") ? user.Substring(1) : user;
            var command = JAR_CMD + " " + handler + " " + platform + " " + user + " " + id;
            using (var client = new SshClient("ip", "user", "pass"))
            {

                client.Connect();
                var infor = client.ConnectionInfo;
                var sshCommand = client.CreateCommand(command);
                sshCommand.Execute();
                result = sshCommand.Result;
                client.Disconnect();
            }
            return result;
        }

        public static string UpdateToken()
        {
            var result = "";
            var command = JAR_CMD + " " + TOKEN;
            using (var client = new SshClient("ip", "user", "pass"))
            {

                client.Connect();
                var infor = client.ConnectionInfo;
                var sshCommand = client.CreateCommand(command);
                sshCommand.Execute();
                result = sshCommand.Result;
                client.Disconnect();
            }
            return result;
        }

    }
}
