# Petto — Rediseño Glassmorphism

**Fecha:** 2026-05-02  
**Estado:** Aprobado por usuario  
**Alcance:** Solo visual — la funcionalidad (bindings, comandos, navegación, validaciones) no cambia.

---

## 1. Objetivo

Reemplazar el diseño actual (fondo blanco/cyan claro, frames planos, colores hardcodeados) con un estilo **glassmorphism moderno**: fondos con gradiente teal-azul oscuro, tarjetas traslúcidas con borde blanco suave, botones con gradiente, tipografía clara sobre oscuro. Se mantiene la paleta base ya definida en `App.xaml`.

## 2. Sistema de colores

### Colores base (ya en App.xaml — no cambiar sus claves)

| Clave              | Hex       | Uso actual         |
|--------------------|-----------|--------------------|
| `PrimaryBackground`| `#D5F9FF` | Texto primario claro |
| `AccentCyan`       | `#26C6DA` | Acentos, bordes activos |
| `AccentBlue`       | `#0288D1` | Botones, gradiente |
| `TextPrimary`      | `#1E1E1E` | (se reemplaza por blanco en pantallas oscuras) |
| `TextSecondary`    | `#607D8B` | (se reemplaza por rgba(213,249,255,0.6)) |
| `ErrorRed`         | `#E53935` | Validaciones de error |
| `SuccessGreen`     | `#A5D6A7` | Indicadores completados |

### Colores nuevos a agregar en App.xaml

> **Formato MAUI:** Los colores transparentes usan `#AARRGGBB` donde AA es el alpha en hex (00=transparente, FF=opaco). Ejemplos: blanco 18% = `#2EFFFFFF`, blanco 35% = `#59FFFFFF`.

| Clave                   | MAUI `#AARRGGBB`  | Uso |
|-------------------------|-------------------|-----|
| `GradientStart`         | `#FF004D5E`       | Inicio del gradiente de fondo |
| `GradientMid`           | `#FF006064`       | Punto medio del gradiente |
| `GradientEnd`           | `#FF0288D1`       | Final del gradiente de fondo |
| `GlassCardBackground`   | `#2EFFFFFF`       | Fondo de tarjetas glass (blanco 18%) |
| `GlassCardBorder`       | `#59FFFFFF`       | Borde de tarjetas glass (blanco 35%) |
| `GlassHeaderBackground` | `#1FFFFFFF`       | Fondo del header glass (blanco 12%) |
| `GlassInputBackground`  | `#26FFFFFF`       | Fondo de campos Entry (blanco 15%) |
| `TextOnDark`            | `#FFD5F9FF`       | Texto primario sobre fondo oscuro |
| `TextOnDarkSecondary`   | `#99D5F9FF`       | Texto secundario (cyan 60% alpha) |
| `TabBarBackground`      | `#4D000000`       | Fondo del tab bar (negro 30%) |
| `DangerGlass`           | `#33E53935`       | Fondo botón cerrar sesión (rojo 20%) |
| `DangerGlassBorder`     | `#66E53935`       | Borde botón cerrar sesión (rojo 40%) |
| `DangerText`            | `#FFFF6B6B`       | Texto botón cerrar sesión |

> **Nota MAUI:** El efecto `backdrop-filter: blur()` no existe en MAUI nativo. El glass se logra con `BackgroundColor` semitransparente + `BorderColor` blanco suave + `Shadow`. No se necesita ningún paquete externo.

## 3. Fondo global

Todas las páginas usan un **LinearGradientBrush** vertical:

```
#004d5e (0%) → #006064 (40%) → #0288D1 (100%)
```

Se aplica como `Background` en el `Grid` raíz de cada página, reemplazando los fondos blancos y cyan claros actuales.

## 4. Componentes rediseñados

### 4.1 Header (todas las páginas excepto Login/Registro)

**Actual:** Frame/Grid con `BackgroundColor="#E0F7FA"` o `#D5F9FF`  
**Nuevo:**
- `BackgroundColor` = `GlassHeaderBackground` (blanco 12% alpha)
- Borde inferior: `#FFFFFF` 20% alpha, grosor 1
- `TextColor` = `#FFFFFF`
- Iconos de menú/perfil: blancos

### 4.2 Tarjetas / Cards

**Actual:** `Frame` con `BackgroundColor="White"`, `HasShadow="False"`, `CornerRadius="12"`  
**Nuevo:**
- `BackgroundColor` = `GlassCardBackground` (blanco 18% alpha)
- `BorderColor` = `GlassCardBorder` (blanco 35% alpha)  
- `CornerRadius` = 14
- `Shadow`: `Color="#000000"`, `Opacity=0.2`, `Radius=12`, `Offset=(0,4)`

### 4.3 Botón primario ("Entrar", "Guardar", "Registrarse", "Enviar", "+ Nueva tarea")

**Actual:** `BackgroundColor="#006064"`, borde-radius 8  
**Nuevo:**
- `Background` = `LinearGradientBrush` horizontal: `#0288D1` → `#26C6DA`
- `TextColor` = `#FFFFFF`
- `CornerRadius` = 22
- `HeightRequest` = 48
- `FontAttributes` = Bold

### 4.4 Botón secundario / outline

**Actual:** varios estilos inconsistentes  
**Nuevo:**
- `BackgroundColor` = blanco 12% alpha
- `BorderColor` = blanco 40% alpha
- `TextColor` = `#FFFFFF`
- `CornerRadius` = 22

### 4.5 Botón de peligro (Cerrar sesión)

**Actual:** `BackgroundColor="#D32F2F"`  
**Nuevo:**
- `BackgroundColor` = `DangerGlass` (rojo 20% alpha)
- `BorderColor` = `DangerGlassBorder` (rojo 40% alpha)
- `TextColor` = `#FF6B6B`
- `CornerRadius` = 22

### 4.6 Campos de entrada (Entry)

**Actual:** `BackgroundColor="White"`, borde `#D9D9D9`  
**Nuevo:**
- `BackgroundColor` = blanco 15% alpha
- `PlaceholderColor` = blanco 50% alpha
- `TextColor` = `#FFFFFF`
- Borde: blanco 35% alpha

### 4.7 Labels

- Labels primarios: `TextColor="#D5F9FF"`
- Labels secundarios (subtítulos, metadatos): `TextColor` = blanco 60% alpha
- Labels de sección uppercase: `TextColor="#26C6DA"`, `FontSize=10`, `FontAttributes=Bold`, `CharacterSpacing=1.5`

## 5. Páginas — cambios específicos

### Login & Registro
- Fondo: gradiente `#0288D1 → #26C6DA → #D5F9FF` (azul arriba, cyan claro abajo)
- Hero superior: bloque con `BackgroundColor` azul 60% alpha, logo "🐾 Petto" blanco 28pt bold
- Formulario: card glass con campos glass
- Enlace "¿No tienes cuenta?": texto blanco 60%, parte clickeable `#D5F9FF` bold

### MainPage (Inicio)
- Eliminar el `BackgroundColor="#E0F7FA"` del header y el fondo blanco del body
- Card central del pet: glass card con avatar (gradiente circular), nombre bold `#D5F9FF`, progress bar gradiente
- Círculos de acción: `BackgroundColor` con alpha 30% del color temático de cada categoría (azul, verde, naranja, púrpura, rojo), borde mismo color 60% alpha
- Label "Puntos de cuidado": texto `#D5F9FF`, número 24pt bold

### Tareas
- Lista "Tareas próximas": items dentro de glass card; dots de color por categoría; badge "HOY"/"VIE" en glass cyan
- Grid de categorías: pills glass con icono emoji (14pt) + nombre (8pt) bold

### Chat
- Burbuja usuario (derecha): `LinearGradientBrush` `#0288D1→#26C6DA`, texto blanco, radio 14/14/4/14
- Burbuja IA (izquierda): glass card, texto `#D5F9FF`, radio 14/14/14/4
- Input area: glass card contenedora; Entry glass; botón enviar: círculo gradiente

### Estadísticas
- DatePicker row: glass card con icono 📅
- Gráfica placeholder: glass card con barras decorativas en gradiente
- 4 cards: glass base + tint de color temático (cyan, naranja `rgba(255,167,38,0.15)`, púrpura `rgba(171,71,188,0.15)`)

### Perfil
- Avatar hero: glass card centrada; botón editar como círculo gradiente superpuesto (bottom-right)
- Info rows: separadas por `BoxView` blanco 12% alpha; campo teléfono editable con glass input

### Configuración
- Secciones separadas con label uppercase cyan
- Picker de idioma: glass card con ▼ decorativo
- Campos de contraseña: glass inputs dentro de glass card

### AppShell (Flyout)
- `FlyoutBackgroundColor`: `rgba(0,0,0,0.35)` sobre el gradiente global (el fondo de la app se ve detrás)
- FlyoutHeader: avatar glass + nombre/email blancos
- Items activos: `BackgroundColor` cyan 20% alpha, borde cyan 35%, texto `#D5F9FF`
- Items inactivos: texto blanco 60% alpha
- Separador: `BoxView` gradiente cyan → transparente
- Footer "Cerrar sesión": botón peligro glass

## 6. Estilos globales en App.xaml

Los estilos de `Button`, `Entry`, y `Label` en `App.xaml` se actualizan para reflejar los valores por defecto del nuevo diseño. Los estilos específicos por página se aplican inline o con `Style x:Key`.

## 7. Fuera de alcance

- No se cambia ningún ViewModel, Command, binding, o validación lógica
- No se cambia la navegación ni la estructura del Shell
- No se agrega ninguna dependencia/paquete externo
- No se cambia ningún archivo de code-behind (.xaml.cs)
- El soporte para modo oscuro/claro (AppThemeBinding) se deja tal como está

## 8. Archivos afectados

1. `App.xaml` — nuevos colores, estilos globales actualizados
2. `AppShell.xaml` — flyout y shell styling
3. `Views/Login.xaml`
4. `Views/Registro.xaml`
5. `Views/MainPage.xaml`
6. `Views/Tareas.xaml`
7. `Views/Chat.xaml`
8. `Views/Estadisticas.xaml`
9. `Views/Perfil.xaml`
10. `Views/Configuracion.xaml`
