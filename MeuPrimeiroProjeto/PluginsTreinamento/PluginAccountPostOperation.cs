using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginsTreinamento
{
    public class PluginAccountPostOperation : IPlugin
    {
        // Método requerido para execução do plugin recebendo como parâmetro os dados do provedor de serviço

        public void Execute(IServiceProvider serviceProvider) 
        {
            try // tentativa de execução
            {
                // Variavel contendo o contexto da execução
                IPluginExecutionContext context =
                    (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // Variavel contendo o service Factory da organização
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService (typeof(IOrganizationServiceFactory));

                // Variavel contendo o service admin que estabelece os serviços de conexão com o dataverse
                IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

                // Variavel do Trace que armazena informações de LOG
                ITracingService trace = (ITracingService)serviceProvider.GetService (typeof(ITracingService));

                // verifica se contem dados para o destino e se corresponde a uma entity
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    // Variavel do tipo Entity herdando a entidade do contexto
                    Entity entidadeContexto = (Entity)context.InputParameters["Target"];

                    if (!entidadeContexto.Contains("websiteurl")) // verifica se o atributo telephone1 não está presente no contexto
                    {
                        throw new InvalidPluginExecutionException("Campo websiteurl é obrigatorio!"); // exibe Exception de erro
                    }

                    // Variavel para nova entidade TASK vazia
                    var Task = new Entity("task");

                    //atribuições dos atributos para novo registro da entidade TASK
                    Task.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);
                    Task.Attributes["regardingobjectid"] = new EntityReference("account", context.PrimaryEntityId);
                    Task.Attributes["subject"] = "Visite nosso site: " + entidadeContexto["websiteurl"];
                    Task.Attributes["description"] = "TASK criada via Plugin Post Operation";

                    serviceAdmin.Create(Task); // executa o método Create para entidade TASK

                }

            }
            catch (InvalidPluginExecutionException ex) // obter exceção caso falha
            {

                throw new InvalidPluginExecutionException("Erro ocorrido: " + ex.Message);
            }
        }   
    }
}
