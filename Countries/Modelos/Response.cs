using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Modelos
{
    public class Response
    {
        // Para verificar se existe internet, se api carregou bem e se os dados foram gravadps na Base de Dados
        public bool IsSucess { get; set; }

        // se algo correr mal vai dar mensagens
        public string Message { get; set; }

        // se tudo correr bem vai guardar um objecto main, serve para qualquer ocorrencia (objecto? cambios4 4:35) 
        public object Result { get; set; }
    }
}
