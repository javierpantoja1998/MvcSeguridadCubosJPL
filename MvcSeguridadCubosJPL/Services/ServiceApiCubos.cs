using MvcSeguridadCubosJPL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MvcSeguridadCubosJPL.Services
{
    public class ServiceApiCubos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiCubos;

        public ServiceApiCubos(IConfiguration configuration)
        {
            this.UrlApiCubos =
                configuration.GetValue<string>("ApiUrls:ApiOAuthCubos");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync
            (string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    Email = email,
                    Password = password
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //METODOS LIBRES

        //METODO PARA SACAR TODOS LOS CUBOS
        public async Task<List<Cubo>> GetCubosAsync()
        {
            string request = "api/Cubos/";
            List<Cubo> cubos =
                await this.CallApiAsync<List<Cubo>>(request);
            return cubos;
        }

        //METODO PARA BUSCAR CUBO
        public async Task<Cubo> FindCuboAsync(string marca)
        {
            string request = "api/Cubos/" + marca;
            Cubo cubo = await this.CallApiAsync<Cubo>(request);
            return cubo;
        }



        //METODO PARA CREAR UN NUEVO CUBO
        public async Task InsertCuboAsync
        (int id, string nombre,
            string marca, string imagen, int precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/cubos";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //TENEMOS QUE ENVIAR UN OBJETO JSON
                //NOS CREAMOS UN OBJETO DE LA CLASE DEPARTAMENTO
                Cubo cubo = new Cubo();
                cubo.IdCubo = id;
                cubo.Nombre = nombre;
                cubo.Marca = marca;
                cubo.Imagen = imagen;
                cubo.Precio = precio;
                //CONVERTIMOS EL OBJETO A JSON
                string json = JsonConvert.SerializeObject(cubo);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }



        //METODO PARA CREAR UN NUEVO USUARIO
        public async Task InsertUsuarioAsync
        (int id, string nombre,
            string email, string password, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/usuarios";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //TENEMOS QUE ENVIAR UN OBJETO JSON
                //NOS CREAMOS UN OBJETO DE LA CLASE DEPARTAMENTO
                Usuario user = new Usuario();
                user.IdUsuario = id;
                user.Nombre = nombre;
                user.Email = email;
                user.Password = password;
                user.Imagen = imagen;
                //CONVERTIMOS EL OBJETO A JSON
                string json = JsonConvert.SerializeObject(user);
               
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        //METODO PARA CREAR UN NUEVO CUBO
        public async Task InsertCuboAsync
        (int id, string nombre,
            string email, string password, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/usuarios";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //TENEMOS QUE ENVIAR UN OBJETO JSON
                //NOS CREAMOS UN OBJETO DE LA CLASE DEPARTAMENTO
                Usuario user = new Usuario();
                user.IdUsuario = id;
                user.Nombre = nombre;
                user.Email = email;
                user.Password = password;
                user.Imagen = imagen;
                //CONVERTIMOS EL OBJETO A JSON
                string json = JsonConvert.SerializeObject(user);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }


        //METODOS PROTEGIDOS

        //METODO PROTEGIDO PARA RECUPERAR EL PERFIL
        public async Task<Usuario> GetPerfilUsuarioAsync
            (string token)
        {
            string request = "/api/Usuarios/PerfilUsuario";
            Usuario user = await
                this.CallApiAsync<Usuario>(request, token);
            return user;
        }

    }
}
