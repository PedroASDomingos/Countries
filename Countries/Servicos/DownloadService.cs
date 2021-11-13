using Countries.Modelos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Countries.Servicos
{
    public class DownloadService
    {
        private DialogService dialogService;
        public async Task GetFlags(List<Country> listofCountries)
        {
            dialogService = new DialogService();
            await Task.Run(() =>
            {
                try
                {
                    foreach (Country country in listofCountries)
                    {
                        string baseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        baseDir = baseDir + "\\Flags\\" + country.name + ".svg";

                        //Download bandeiras
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(country.flag, baseDir);
                        }
                    }
                }
                catch (Exception e)
                {
                    dialogService.ShowMessage("Error downloading flags", e.Message);
                }
            });
        }
    }
}
