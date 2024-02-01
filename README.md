# Prueba técnica febrero 2024
Prueba técnica de conocimientos en Xamarin.

# Enunciado


 En este ejercicio se desea encontrar solución a dos "bugs" de una pequeña aplicación multiplataforma desarrollada con Xamarin Forms, además de añadir algunas funcionalidad extra.
 Esta aplicación consta de una única pantalla en la cual se encuentran un listado de usuarios y un botón para añadir un usuario nuevo.

 El código fuente puede ser modificado completamente tal y como desee el desarrollador, pero se deben respetar las reglas de la arquitectura MVVM y se valoran principios de desarrollo SOLID.

 Si se tiene cualquier duda sobre alguno de los bugs, funcionalidades o cualquier otro aspecto no dudes en preguntar.

 ### Los bugs a resolver serían los siguientes:

 1-La aplicación tiene un botón de añadir usuario no funciona correctamente. Parece que el usuario se añade al listado en memoria pero ese cambio no se ve representado en el listado de la pantalla. El nuevo 
 usuario debería aparecer al final del listado actual, sin resetear ni modificar los usuarios que ya existen en el listado previamente.

 2-En el listado de usuario aparecen el nombre y apellido del usuario, pero estos están demasiado separados. Deberían aparecer seguidos el uno del otro, independientemente de la longitud de los mismos.

### Nuevas funcionalidades

 1-Cada usuario puede estar seleccionado o no en el listado. Existe una propiedad de tipo boolean llamada "Seleccionado" en el modelo que identifica ese estado. Dependiendo del valor de esa propiedad "Seleccionado" se debe cambiar el color de fondo de cada Usuario de la lista. Los seleccionados deben tener un color y los no seleccionado otro diferente. Rojo los no seleccionados y verde los seleccionados.

 2-Cuando se pulse en un usuario de la lista, el estado Seleccionado debe cambiar a su opuesto. Es decir, si Seleccionado es true pasará a ser false y viceversa. El color de fondo deberá cambiar en consecuencia, sin reiniciar ni cambiar el estado del resto de usuarios de la lista.

 3-Cuando se haga scroll del listado y se esté llegando al final del mismo deberán añadirse más usuarios a la lista, generando de esta manera un listado de scroll infinito. Se pueden volver a añadir los mismos usuarios de test que devuelve la función "getUsuarios()" que ya se encuentra en el proyecto. Siempre sin modificar ni resetear los usuarios ya presentes en la lista.

# Solución
## 1. Refactorización inicial.
Realizada en la rama [initial_refactoring](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/tree/initial_refactoring).

Tras adecuar la configuración del proyecto, se ha llevado a cabo una reorganización en carpetas más típica con MVVC.

La funcionalidad de carga de usuarios se ha movido del viewModel a un [servicio](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/blob/dev/TestAptitudes/TestAptitudes/Services/UsuarioService.cs) con el objetivo de separar responsabilidades. Además, dado que servicios como este se suelen utilizar para cargar datos de disco, internet o base de datos se ha configurado como asíncrono, para evitar acumular deuda técnica. El servicio devuelve datos mockup, puede emular un retraso en obtener los datos y en un futuro será fácil de adecuar a una factoría.

Se han cambiado ligeramente la nomenclatura de las funciones en el viewModel y se ha creado un nuevo comando para cargar los usuarios, lo cual será útil en un punto posterior.

### Inyección de dependencias
Sería interesante utilizar inyección de dependencias para el Servicio y quizá para el viewModel. .Net Maui viene con un sistema de inyección de dependencias integrado que facilita mucho realizarlas. Sin embargo, dado que Xamarin no lo tiene, se suele implementar esta funcionalidad importando un paquete Nuget que la provea. En este caso, dado el tamaño del proyecto, considero que no está justificado el esfuerzo de desarrollo y la adición de una nueva dependencia al mismo (más allá de dejar constancia en esta nota).

## 2. Solución de bugs.
Relizada en la rama [bux_fixing](https://github.com/salvadorjesus/Prueba-tecnica-febrero-2024/tree/dev).
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
