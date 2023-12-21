using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace PluginsTreinamento
{
    public class PluginAccountPreValidation : IPlugin
    {
        //método requirido para execução do plugin recebendo como parâmetro os dados do provedor(Dynamics) de serviço

        public void Execute(IServiceProvider serviceProvider)
        {
            // Variavel contendo o contexto da execução | context -> formulário do dynamics
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Variavel contendo o service Factory da organização | informações da base da organização
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService (typeof(IOrganizationServiceFactory));

            // Variavel contendo o Service Admin que estabelece os serviços de conexão com o dataverse | esta função subistitui a classe conection
            IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

            // Variavel do Trace que armazena informações de LOG 
            ITracingService trace = (ITracingService)serviceProvider .GetService (typeof(ITracingService));

            // Variavel do tipo Entity vazia
            Entity entidadeContexto = null;

            if (context.InputParameters.Contains("Target")) // Verifica se contém dados para o destino
            {
                entidadeContexto = (Entity)context.InputParameters["Target"]; // atribui o contexto da entidade para a variavel

                trace.Trace("Entidade do contexto: " + entidadeContexto.Attributes.Count); // armazena infomrações de LOG

                if (entidadeContexto == null) // Verifica se a entidade do contexto está vazia
                {
                    return;
                }
                if (!entidadeContexto.Contains("telephone1")) // verifica se o atributo telephone1 não esta presente no contexto | verifica se o telefone não foi declarado
                {
                    throw new InvalidPluginExecutionException("Campo Telefone principal é obrigatório!");
                }
            }
        }
    }
}
