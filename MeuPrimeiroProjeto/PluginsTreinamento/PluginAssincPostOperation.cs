using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginsTreinamento
{
    public class PluginAssincPostOperation : IPlugin
    {
        // Metodo requirido para execucao do plugin recebendo como parametro os dados do provedor de servico

        public void Execute(IServiceProvider serviceProvider)
        {
            try // tentativa de execução
            {
                // Variavel contendo o contexto da execucao
                IPluginExecutionContext context =
                    (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // Variavel contendo o service factory da organização
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                // Variavel contendo o service Admin que estabelece os serviços de conexão com o dataverse
                IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

                // Variavel do Trace que armazena informaçoes de LOG
                ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                // Verifica se contem dados para o destino e se corresponde a uma entity
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    // Variavel do tipo Entity herdando a entidade do contexto
                    Entity entidadeContexto = (Entity)context.InputParameters["Target"];

                    for (int i = 0; i < 10; i++)
                    {
                        // Variavel para nova entidade contato vazia
                        var Contact = new Entity("contact");

                        // atribuição dos atributos para novo registro da entidade contato
                        Contact.Attributes["firstname"] = "Contato Assinc vinculado a conta";
                        Contact.Attributes["lastname"] = entidadeContexto["name"];
                        Contact.Attributes["parentcustomerid"] = new EntityReference("account", context.PrimaryEntityId);
                        Contact.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);

                        trace.Trace("firstname: " + Contact.Attributes["firstname"]);

                        serviceAdmin.Create(Contact); // executa o metodo create para entidade contato
                    }
                }

            }
            catch (InvalidPluginExecutionException ex) // caso falha
            {

                throw new InvalidPluginExecutionException("Erro ocorrido: " + ex.Message);
            }
        }
    }
}
