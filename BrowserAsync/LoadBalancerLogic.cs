using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
/* Enlaces de interés 
 * 
     https://geeks.ms/etomas/2011/09/17/c-5-async-await/
     https://stackoverflow.com/questions/9343594/how-to-call-asynchronous-method-from-synchronous-method-in-c
     https://briancaos.wordpress.com/2017/11/03/using-c-httpclient-from-sync-and-async-code/
     https://medium.com/bynder-tech/c-why-you-should-use-configureawait-false-in-your-library-code-d7837dce3d7f

 */
namespace BrowserAsync
{
    class LoadBalancerLogic
    {
        /* Esta propiedad es opcional porque no sería imprescindible saber desde el instante 0
         cuántos balanceradores de carga hay en el sistema. Se puede realizar una petición asíncrona
         para averiguarlo y si CURRENT_NUM_LOAD_BALANCERS es nulo en el instante de hacer peticiones POST para
         almacenar las palabras, se puede utilizar en su lugar DEFAULT_NUM_LOAD_BALANCERS */
        private static int? CURRENT_NUM_LOAD_BALANCERS;

        /* Si el patrón por defecto tuviese que modificarse,
        * sería necesario actualizar todas las apps de escritorio.
        * Se supone que debe conocerse este patrón desde el principio y 
        * sin posibilidad de modificación */
        private static readonly string DEFAULT_URI_LOAD_BALANCERS = "https://Xloadbalancerbrowserapi.azurewebsites.net/api/postDataToApi";

        /*Como mínimo el sistema va a contar con 2 balanceadores de carga*/
        private static readonly int DEFAULT_NUM_LOAD_BALANCERS = 2;

        /* Esta lista contiene las URLs de todos los balanceadores de carga disponibles en el sistema.
         La idea sería que se actualizase periódicamente al menos antes de leer un documento y compruebe el 
         estado de CURRENT_NUM_LOAD_BALANCERS*/
        private static List<string> LOAD_BALANCERS_LIST = new List<string>();


        private static readonly string URL_GET_NUMBER_BALANCERS_COMPONENT = "https://browserapinumloadbal.azurewebsites.net/api/GetNumLoadBalancers";


        // Este método debe ser llamado antes de indexar un documento.
        private void updateLoadBalancersListAsync()
        {
            try {
                CURRENT_NUM_LOAD_BALANCERS = GetNumLoadBalancersAsync().Result;
            } catch
            {
                CURRENT_NUM_LOAD_BALANCERS = DEFAULT_NUM_LOAD_BALANCERS;
            }

            for(int i = 1; i <= CURRENT_NUM_LOAD_BALANCERS; i++)
            {
                string url = DEFAULT_URI_LOAD_BALANCERS.Replace("X", i.ToString());
                LOAD_BALANCERS_LIST.Add(url);
            }
        }

        /*Este es un método asíncrono de tipo Task que devolverá el número 
         * actual de balanceadores del sistema mediante una petición GET a 
         una FunctionApp desplegada en Microsft Azure*/
        private async Task<int> GetNumLoadBalancersAsync()
        {
            var _httpClient = new HttpClient();

            using (var result = await _httpClient.GetAsync(URL_GET_NUMBER_BALANCERS_COMPONENT).ConfigureAwait(false))
            {
                string content = await result.Content.ReadAsStringAsync();
                return int.Parse(content);
            }
        }






        /** Puesta en marcha del balanceador */
        /* Una vez tenemos la lista de palabras de un documento y hemos intentado actualizar
         el valor de CURRENT_NUM_LOAD_BALANCERS, definimos el comportamiento del balanceador.
         Para ello suponemos que tenemos en la siguiente lista todas las palabras del documento.*/


        // updateLoadBalancersList();
        //(Ya hemos actualizado CURRENT_NUM_LOAD_BALANCERS)

        List<string> allWordsInDocument = new List<string>();

        private void postWordsToLoadBalancers()
        {
            for (int i = 0; i < allWordsInDocument.Count; i++)
            {
                PostWord(LOAD_BALANCERS_LIST.ElementAt(i % CURRENT_NUM_LOAD_BALANCERS.Value), allWordsInDocument.ElementAt(i));
            }

        }

        private void PostWord(string loadBalancerURL, string word)
        {
            throw new NotImplementedException();
        }
    }
   
 }

