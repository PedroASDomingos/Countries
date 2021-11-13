﻿namespace Countries.Servicos
{
    using Countries.Modelos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        public async Task<Response> GetInfo(string urlbase, string controller)
        {
            try
            {
                var client = new HttpClient();
                //liga-se a api
                client.BaseAddress = new Uri(urlbase);
                //controlador api
                //fica a espera da resposta da api
                var response = await client.GetAsync(controller);
                //carrega resultados no formato de uma string para o result
                //fica a espera da resposta do controlador
                var result = await response.Content.ReadAsStringAsync();
                //verifica se nao teve sucesso
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result,
                    };
                }
                //caso tenha sido sucesso

                var info = JsonConvert.DeserializeObject<List<Country>>(result);

                return new Response
                {
                    IsSucess = true,
                    Result = info,
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = e.Message,
                };
            }
        }

        public async Task<Response> GetInfoAPI(string urlbase, string controller)
        {
            try
            {
                var client = new HttpClient();
                //liga-se a api
                client.BaseAddress = new Uri(urlbase);
                //controlador api
                //fica a espera da resposta da api
                var response = await client.GetAsync(controller);
                //carrega resultados no formato de uma string para o result
                //fica a espera da resposta do controlador
                var result = await response.Content.ReadAsStringAsync();
                //verifica se nao teve sucesso
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result,
                    };
                }
                //caso tenha sido sucesso

                var info = JsonConvert.DeserializeObject<CountryInfo>(result);

                return new Response
                {
                    IsSucess = true,
                    Result = info,
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = e.Message,
                };
            }
        }
    }
}
