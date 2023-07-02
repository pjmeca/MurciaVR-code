## Despliegue

- Para hacer funcionar la aplicación debe generarse una clave en: https://www.bingmapsportal.com y colocarla en la API de Bing. 

**¡Importante!** Debe crearse el archivo: `Assets/Resources/MapSessionConfig.txt` e introducir la clave ahí. No debe colocarse en el Inspector, ya que hay varios scripts que la utilizan y dependen de este archivo.

- El proyecto utiliza Zenject para la inyección de dependencias, debe instalarse desde la Asset Store de Unity o ubicarse dentro del directorio `Assets/Plugins`.
- También utiliza [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) para poder usar [KdTree](https://github.com/codeandcats/KdTree).

## Grabaciones del desarrollo

- Demostración del movimiento: [TFG #1 - Prototipo Movimiento](https://youtu.be/V6wA9JmB0KM)

- Primera implementación de Murcia: [TFG #2 - MURCIA en VR (v1)](https://youtu.be/6cK7So_YNYU)

- Mecánica de aspirar la contaminación y sistema de movimiento rehecho: [TFG #3 - Interacción VR: PistolaAspiradora, contaminación y nuevo sistema de movimiento VR](https://youtu.be/ln0BrTZtU_E))

- Demostración de la escena inicial y del funcionamiento del sistema de cinemáticas con multicámaras en tiempo real creado usando Cinemachine: [TFG #4 - Creando una composición multicámara animada en tiempo real en Unity VR con Cinemachine](https://youtu.be/ePMZGqsFqzw)

- Demostración de la build final corriendo nativamente en las Meta Quest 2: [TFG #5 - Demostración de MurciaVR corriendo nativamente en las Meta Quest 2](https://youtu.be/A2XpRTwfmzo)
