using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PluginsTreinamento
{
    public class PluginAccountPreOperation : IPlugin
    {
        // método requerido para execução do Plugin recebendo como parâmetro os dados do provedor de serviço

        public void Execute(IServiceProvider serviceProvider)
        {
            // Variável contendo o contexto da execução
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Variável contendo o service Factory da organização
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Variavel contendo o service Admin que estabelece os serviçõos de conexão do Dataverse
            IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

            // Variavel do trace que armazena informações de LOG
            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Verifica se contém dados para o destino e se corresponde a uma entity
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity) 
            {
                // Variavel do tipo Entity herdando a entidade do contexto
                Entity entidadeContexto = (Entity)context.InputParameters["Target"];

                if (entidadeContexto.LogicalName == "account") // verifica se a entidade do contexto é um account
                {
                    if (entidadeContexto.Attributes.Contains("teelephone1")) // verifica se contem o atributo telephone 1
                    {
                        // variavel para herdar o conteudo do atributo telephone1 do contexto
                        var phone1 = entidadeContexto["telephone1"].ToString();

                        // variavel string contendo FetchXML para consulta do contato
                        string FetchContact = @"<?xml version='1.0'?>" +
                        "<fetch distinct='false' mapping='logical' output-format='xml-plataform' version='1.0'>" +
                        "<entity name='contact'>" +
                        "<attribute name='fullname'/>" +
                        "<attribute name='telephone1'/>" +
                        "<attribute name='contactid'/>" +
                        "<order descending='false' attribute='fullname'/>" +
                        "<filter type='and'>" +
                        "<condition attribute='telephone1' value='" + phone1 + "' operator='eq'/>" +
                        "</filter>" +
                        "</entity>" +
                        "</fetch>";

                        trace.Trace("FetchContact: " + FetchContact); // armazena informações de LOG

                        // variavel contendo o retorno da consulta FetchXML
                        var primarycontact = serviceAdmin.RetrieveMultiple(new FetchExpression(FetchContact));

                        if (primarycontact.Entities.Count > 0) // verifica se contem entidade
                        {
                            // para cada entidade retornada atribui a variavel entityContact
                            foreach (var entityContact in primarycontact.Entities)
                            {
                                // atribui referencia de entidade para o atributo primarycontactid (contato primario)
                                entidadeContexto["primarycontactid"] = new EntityReference("contact", entityContact.Id);
                            }

                        }
                    }
                }

            }
        }
    }
}
