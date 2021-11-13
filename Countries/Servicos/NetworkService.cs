namespace Countries.Servicos
{
    using Countries.Modelos;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class NetworkService
    {
        public Response CheckConnection()
        {
            var client = new WebClient();
            try
            {
                //testar ligação à internet, pingando o servidor da google
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSucess = true,
                    };
                }
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = "Configure your Internet connection..."
                };
            }
        }
    }
}
