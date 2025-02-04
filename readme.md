# 🎮 Laberinto Estratégico - Juego de Consola

Un juego de laberinto multijugador con habilidades especiales y gestión de recursos. ¡Llega a la base enemiga antes que tu oponente!

## 🌟 Características Principales
- **Generación procedural de laberintos** (40x40 celdas)
- **Sistema de 3 vidas** con trampas aleatorias
- **5 clases de personajes** con habilidades únicas
- **Movimiento táctico** con sistema de cooldowns
- **Detección de caminos accesibles** (flood fill algorithm)
- **Renderizado en capas** (terreno + jugadores)
- **Interfaz de consola interactiva**

🕹️ Controles
Acción	Teclas
Movimiento	↑↓←→ / W (arriba) A (izquierda) S(abajo) D(derecha)
Usar habilidad	Barra espaciadora
Salir	Ctrl + C

🧙 Clases Disponibles
Personaje	Habilidad	Cooldown	Duración	Símbolo
Warrior(Guerrero)	Escudo	5	2	WA
Mage(Mago)	Curación	6	1	MA
Rogue(Bribón)	Velocidad	4	3	RO
Archer(Arquero)	Alcance	5	2	AR
Necro	Revivir	10	1	NE
🏆 Mecánicas del Juego
Generación del Laberinto:

Paredes perimetrales

20 obstáculos aleatorios (🌳)

15 trampas ocultas (💀/🔥/⚠️)

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
