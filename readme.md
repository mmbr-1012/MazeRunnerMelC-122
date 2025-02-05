# 🎮 Laberinto Embrujado - Juego de Consola

Un juego de laberinto multijugador con habilidades especiales y gestión de recursos. ¡Llega a la base enemiga antes que tu oponente!

## 🌟 Características Principales
- **Generación procedural de laberintos** (121x31 celdas)
- **Sistema de 3 vidas** con trampas aleatorias
- **5 clases de personajes** con habilidades únicas
- **Movimiento táctico** con sistema de cooldowns
- **Detección de caminos accesibles** (flood fill algorithm)
- **Renderizado en capas** (terreno + jugadores)
- **Interfaz de consola interactiva**

🕹️ Controles
Acción	Teclas
Movimiento	↑↓←→ / WASD
Usar habilidad	Barra espaciadora
Salir	Ctrl + C

🧙 Clases Disponibles
Personaje	Habilidad	
Warrior	Escudo	
Mage	Curación		
Rogue	Velocidad		
Archer	Alcance		
Necro	Revivir		
🏆 Mecánicas del Juego
Generación del Laberinto:

Paredes perimetrales

Obstáculos aleatorios

Trampas ocultas

Caminos garantizados (flood fill)

Sistema de Trampas:

Reducen 1 vida al activarse

Símbolos aleatorios revelados al pisar

Posicionamiento estratégico

Victoria:

Alcanzar la posición inicial del oponente

Eliminar al oponente (vidas = 0)

📌 Reglas Clave
Movimiento restringido a celdas vacías

Habilidades consumen turno

Trampas reveladas permanecen visibles

Cooldowns globales por habilidad

Posiciones iniciales fijas (esquinas opuestas)

🛡️ Sistema de Habilidades
Habilidades afectan mecánicas de movimiento

Efectos visuales en consola

Gestión de estado por turnos