using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreateUserFields.Languages
{
    public class LanguageService
    {
        public static string Label(LabelEnum labelEnum, CultureInfo cultureInfo)
        {
            try
            {
                return ResourceMgr.GetString(labelEnum.ToString(), cultureInfo);
            }
            catch
            {
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("pt-BR"))
                    throw new Exception($"Não foi definida mensagem no arquivo de recurso para o enum [{labelEnum}] ");
                else
                    throw new Exception($"Message text is not set for resource file language for enum [{labelEnum}] ");
            }
        }

        public static ResourceManager _resourceManager;
        public static ResourceManager ResourceMgr
        {
            get
            {
                if (_resourceManager == null)
                    _resourceManager = new ResourceManager("CreateUserFields.Languages.Label", AssemblyMessage);
                return _resourceManager;
            }
            private set { _resourceManager = value; }
        }

        private static Assembly _assembly;

        public static Assembly AssemblyMessage
        {
            get
            {
                if (_assembly == null)
                    _assembly = typeof(LanguageService).Assembly;
                return _assembly;
            }
            private set { _assembly = value; }
        }
    }
}
