# Prueba técnica febrero 2024
Prueba técnica de conocimientos en Xamarin.

# Enunciado

> En este ejercicio se desea encontrar solución a dos "bugs" de una pequeña aplicación multiplataforma desarrollada con Xamarin Forms, además de añadir algunas funcionalidad extra.
> Esta aplicación consta de una única pantalla en la cual se encuentran un listado de usuarios y un botón para añadir un usuario nuevo.
>
> El código fuente puede ser modificado completamente tal y como desee el desarrollador, pero se deben respetar las reglas de la arquitectura MVVM y se valoran principios de desarrollo SOLID.
>
> Si se tiene cualquier duda sobre alguno de los bugs, funcionalidades o cualquier otro aspecto no dudes en preguntar.
>
> ### Los bugs a resolver serían los siguientes:
>
> 1-La aplicación tiene un botón de añadir usuario no funciona correctamente. Parece que el usuario se añade al listado en memoria pero ese cambio no se ve representado en el listado de la pantalla. El nuevo usuario debería aparecer al final del listado actual, sin resetear ni modificar los usuarios que ya existen en el listado previamente.
> 
> 2-En el listado de usuario aparecen el nombre y apellido del usuario, pero estos están demasiado separados. Deberían aparecer seguidos el uno del otro, independientemente de la longitud de los mismos.
>
> ### Nuevas funcionalidades
>
> 1-Cada usuario puede estar seleccionado o no en el listado. Existe una propiedad de tipo boolean llamada "Seleccionado" en el modelo que identifica ese estado. Dependiendo del valor de esa propiedad "Seleccionado" se debe cambiar el color de fondo de cada Usuario de la lista. Los seleccionados deben tener un color y los no seleccionado otro diferente. Rojo los no seleccionados y verde los seleccionados.
>
> 2-Cuando se pulse en un usuario de la lista, el estado Seleccionado debe cambiar a su opuesto. Es decir, si Seleccionado es true pasará a ser false y viceversa. El color de fondo deberá cambiar en consecuencia, sin reiniciar ni cambiar el estado del resto de usuarios de la lista.
>
> 3-Cuando se haga scroll del listado y se esté llegando al final del mismo deberán añadirse más usuarios a la lista, generando de esta manera un listado de scroll infinito. Se pueden volver a añadir los mismos usuarios de test que devuelve la función "getUsuarios()" que ya se encuentra en el proyecto. Siempre sin modificar ni resetear los usuarios ya presentes en la lista.

# Solución
## 1. Refactorización inicial.
Realizada en la rama [initial_refactoring](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/tree/initial_refactoring).

Tras adecuar la configuración del proyecto, se ha llevado a cabo una reorganización en carpetas más típica con MVVC.

La funcionalidad de carga de usuarios se ha movido del viewModel a un [servicio](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/blob/dev/TestAptitudes/TestAptitudes/Services/UsuarioService.cs) con el objetivo de separar responsabilidades. Además, dado que servicios como este se suelen utilizar para cargar datos de disco, internet o base de datos se ha configurado como asíncrono, para evitar acumular deuda técnica. El servicio devuelve datos mockup, puede emular un retraso en obtener los datos y en un futuro será fácil de adecuar a una factoría.

Se han cambiado ligeramente la nomenclatura de las funciones en el viewModel y se ha creado un nuevo comando para cargar los usuarios, lo cual será útil en un punto posterior.

### Inyección de dependencias
Sería interesante utilizar inyección de dependencias para el Servicio y quizá para el viewModel. .Net Maui viene con un sistema de inyección de dependencias integrado que facilita mucho realizarlas. Sin embargo, dado que Xamarin no lo tiene, se suele implementar esta funcionalidad importando un paquete Nuget que la provea. En este caso, dado el tamaño del proyecto, considero que no está justificado el esfuerzo de desarrollo y la adición de una nueva dependencia al mismo (más allá de dejar constancia en esta nota).


## 2. Solución de bugs.
Realizada en la rama [bux_fixing](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/tree/dev).
### La aplicación tiene un botón de añadir usuario no funciona correctamente
La lista del [viewModel](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/blob/bug_fixing/TestAptitudes/TestAptitudes/ViewModel/MainViewModel.cs) que almacena los usuarios no es observable. Como consecuencia, al alterar esta la vista no detecta que tiene que refrescarse. Cambiar la lista por una `ObservableCollection` soluciona el problema.
```
        public ObservableCollection<UsuarioModel> Usuarios { get; set; }
        //...
        public MainViewModel()
        {
            //...
            Usuarios = new ObservableCollection<UsuarioModel>();
            //...
        }
```
### Nombre y apellidos muy separados en el listado
En la descripción de este bug se especifica que nombre y apellidos deben aparecer a continuación, sin importar la longitud de estos. Dado que, en principio, un nombre puede ser más largo que la columna que lo contiene, parece conveniente que nombre y apellidos se presenten en una sola etiqueta. Para ello utilizamos un _multibinding_ desde la vista, ya que la presentación no es responsabilidad del viewModel.
```
                            <Label.Text>
                                <MultiBinding StringFormat="{}{0} {1}">
                                    <Binding Path="Nombre"/>
                                    <Binding Path="Apellido"/>
                                </MultiBinding>
                            </Label.Text>
```
La tabla queda ahora con una sola columna. Es posible que el autor original quiera añadir otros elementos a la celda (como el color, más adelante). Además, mantiene el requisito de altura.

## 3. Implementación de nuevas funcionalidades
Realizada en la rama [feature/test_features](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/tree/feature/test_features).

### El color del usuario depende de si está seleccionado
Para implementar esta funcionalidad hacemos un binding para el background color desde la propiedad `BackgroundColor` de la tabla a la propiedad `Seleccionado` del modelo. Como los tipos de datos son diferentes hacemos uso de un converter: ` SelectedToColorConverter`. Los colores se han definido en el diccionario de la aplicación en `App.xaml`:
```
    <Application.Resources>
        <ResourceDictionary>
            <!--Colors for selected and not selected items-->
            <Color x:Key="SelectedColor">darkgreen</Color>
            <Color x:Key="NotSelectedColor">darkred</Color>
            <Color x:Key="UserTextColor">lightGray</Color>
        </ResourceDictionary>
    </Application.Resources>
```
El converter se añade al diccionario de la página:
```
   <!--Recursos de la página-->
    <ContentPage.Resources>
        <ResourceDictionary>
            <converters:SelectedToColorConverter x:Key="SelectedToColorConverter" />
        </ResourceDictionary>
    </ContentPage.Resources>
```
…
```
<Grid BackgroundColor="{Binding Seleccionado, Converter={StaticResource SelectedToColorConverter}}"
```
### Cuando el usuario pulsa en un usuario de la lista, su estado seleccionado cambia y este cambio se ve reflejado en la página

Para que la vista responda al cambio de estado que efectúa el comando, tenemos varias opciones:

1. Llamar a `CollectionChanged` en la colección de usuarios desde el comando. Esto hace que la vista vuelta a dibujar toda la lista. Además de las posibles implicaciones en el rendimiento, en MVVM la vista debería reaccionar al cambio, no esperar a que le indiquemos que se redibuje. No obstante, es la que menos código añade.
2. Modificar el modelo (`UsuarioModel`) para que implemente la interfaz ` INotifyPropertyChanged`. De esta manera, cambios en el mismo se verán reflejados automáticamente en la vista. Esta es una solución aceptada comúnmente e incluso [recomendada por Microsoft]( https://learn.microsoft.com/en-us/previous-versions/msp-n-p/gg405484(v=pandp.40)?redirectedfrom=MSDN#the-model-class). No obstante, tiene algunos inconvenientes:
    * Desde un punto de vista purista, el Modelo debería encargarse exclusivamente de almacenar los datos. Igualmente, no debería ser responsabilidad del modelo preocuparse de qué hace o qué deja de hacer la vista (separación de responsabilidades).
    * En algunas ocasiones no estará en nuestra mano modificar las clases del modelo.
3. Crear una clase en el viewModel que encapsule al modelo, y usar esta para los bindings con la vista. Esta es otra de las opciones recomendadas por Microsoft en el enlace del punto anterior. Separa mejor responsabilidades y puede resultar útil para separar el estado de los datos en la aplicación y en el modelo. Esto puede tener sentido según la lógica de la aplicación (por ejemplo, el usuario podría modificar datos pero no podría guardarlos hasta que no cumplan ciertas restricciones). Como punto negativo, añade boilerplate y mayor complejidad al viewModel.
  
Decidirse por una opción u otra dependerá del contexto de la aplicación y, posiblemente, de las prácticas acordadas por el equipo. En este caso se implementa la tercera opción ya que atender a los principios SOLID y MVVM es un requisito:

Implementación de la clase ‘UsuarioViewModel’ para encapsular a `UsuarioModel`:
```
public class UsuarioViewModel : INotifyPropertyChanged
    {
        private UsuarioModel _usuario;
…
```
Modificación de `MainViewModel` para utilizar la nueva clase:
```
…
public ObservableCollection<UsuarioViewModel> Usuarios { get; set; }
…
        private async void LoadUsers()
        {
            var usuarios = await UsuarioService.GetUsuarios();
            foreach (var usuario in usuarios)
                Usuarios.Add( new UsuarioViewModel(usuario));
        }
…
```

### Implementación de scroll infinito
Para añadir población a la lista podemos seguir usando el comando ` AddUserListCmd`, ya que crea y añade elementos nuevos, respetando a los anteriores. Para llamar a este comando cuando el scroll de la lista se esté acercando al final podemos usar las propiedades ` RemainingItemsThreshold`:
```
    <StackLayout>
        <CollectionView ItemsSource="{Binding Usuarios}"
                        Margin="20"
                        RemainingItemsThreshold="2"
                        RemainingItemsThresholdReachedCommand="{Binding AddUserListCmd}">
```


