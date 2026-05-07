# Petto Glassmorphism Redesign — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the current flat cyan/white UI with a glassmorphism design — gradient dark-teal backgrounds, translucent glass cards, and gradient buttons — across all 10 XAML views while preserving every binding, command, and validation trigger.

**Architecture:** Pure XAML changes only. Each file is rewritten in place. No `.xaml.cs`, ViewModel, or navigation logic is touched. DataTrigger validation logic is preserved; only the colors in the Setters change. Entry elements are wrapped in a `Border` for glass styling (the DataTrigger moves to the Border parent).

**Tech Stack:** .NET MAUI 8, XAML, `LinearGradientBrush`, `Frame` (glass cards), `Border` (glass inputs & chat bubbles), `DataTrigger`

---

## File Map

| File | Change |
|------|--------|
| `App.xaml` | Add 14 color tokens; add named styles `GlassCard`, `PrimaryButton`, `DangerButton`; update default Button/Entry/Label styles |
| `AppShell.xaml` | Dark flyout background, white text, gradient header, glass footer button, white tab bar colors |
| `Views/Login.xaml` | Login-variant gradient background, hero section, glass form card, glass inputs, gradient button |
| `Views/Registro.xaml` | Same pattern as Login with 4 fields |
| `Views/MainPage.xaml` | Page gradient, glass header, glass pet card with gradient avatar circle, translucent action circles |
| `Views/Tareas.xaml` | Page gradient, glass header, glass task list, glass category pills |
| `Views/Chat.xaml` | Page gradient, glass header, gradient user bubbles, glass AI bubbles, glass input row |
| `Views/Estadisticas.xaml` | Page gradient, glass header, glass chart placeholder, glass stat cards with color tints |
| `Views/Perfil.xaml` | Page gradient, glass header, glass avatar card, glass info card, glass phone input |
| `Views/Configuracion.xaml` | Page gradient, glass header, glass picker card, glass password inputs, glass danger button |

---

## Task 1: App.xaml — Color tokens & global styles

**Files:**
- Modify: `App.xaml`

- [ ] **Step 1: Replace App.xaml with the new version**

Replace the entire content of `App.xaml` with:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="PettoV1.App">
    <Application.Resources>
        <ResourceDictionary>

            <!-- ── Colores base Petto (sin cambiar claves) ── -->
            <Color x:Key="PrimaryBackground">#D5F9FF</Color>
            <Color x:Key="PrimaryDark">#2C2C2C</Color>
            <Color x:Key="AccentCyan">#26C6DA</Color>
            <Color x:Key="AccentBlue">#0288D1</Color>
            <Color x:Key="ErrorRed">#E53935</Color>
            <Color x:Key="SuccessGreen">#A5D6A7</Color>
            <Color x:Key="BorderColor">#D9D9D9</Color>
            <Color x:Key="PlaceholderColor">#B3B3B3</Color>
            <Color x:Key="TextPrimary">#1E1E1E</Color>
            <Color x:Key="TextSecondary">#607D8B</Color>
            <Color x:Key="CardBackground">White</Color>
            <Color x:Key="InvalidFieldBackground">#FFEBEE</Color>

            <!-- ── Nuevos tokens glassmorphism ── -->
            <Color x:Key="GradientStart">#FF004D5E</Color>
            <Color x:Key="GradientMid">#FF006064</Color>
            <Color x:Key="GradientEnd">#FF0288D1</Color>
            <Color x:Key="GlassCardBackground">#2EFFFFFF</Color>
            <Color x:Key="GlassCardBorder">#59FFFFFF</Color>
            <Color x:Key="GlassHeaderBackground">#1FFFFFFF</Color>
            <Color x:Key="GlassInputBackground">#26FFFFFF</Color>
            <Color x:Key="TextOnDark">#FFD5F9FF</Color>
            <Color x:Key="TextOnDarkSecondary">#99D5F9FF</Color>
            <Color x:Key="TabBarBackground">#4D000000</Color>
            <Color x:Key="DangerGlass">#33E53935</Color>
            <Color x:Key="DangerGlassBorder">#66E53935</Color>
            <Color x:Key="DangerText">#FFFF6B6B</Color>
            <Color x:Key="GlassInputInvalid">#4DE53935</Color>

            <!-- ── Gradiente global de fondo ── -->
            <LinearGradientBrush x:Key="PageGradient" StartPoint="0,0" EndPoint="0,1">
                <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
                <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
                <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
            </LinearGradientBrush>

            <!-- ── Gradiente login (más claro) ── -->
            <LinearGradientBrush x:Key="LoginGradient" StartPoint="0,0" EndPoint="0,1">
                <GradientStop Color="#FF0288D1" Offset="0.0"/>
                <GradientStop Color="#FF26C6DA" Offset="0.5"/>
                <GradientStop Color="#FFB2EBF2" Offset="1.0"/>
            </LinearGradientBrush>

            <!-- ── Gradiente botón primario ── -->
            <LinearGradientBrush x:Key="PrimaryButtonGradient" StartPoint="0,0" EndPoint="1,0">
                <GradientStop Color="#FF0288D1" Offset="0"/>
                <GradientStop Color="#FF26C6DA" Offset="1"/>
            </LinearGradientBrush>

            <!-- ── Estilo Frame glass (GlassCard) ── -->
            <Style x:Key="GlassCard" TargetType="Frame">
                <Setter Property="BackgroundColor"  Value="{StaticResource GlassCardBackground}"/>
                <Setter Property="BorderColor"      Value="{StaticResource GlassCardBorder}"/>
                <Setter Property="CornerRadius"     Value="14"/>
                <Setter Property="Padding"          Value="12"/>
                <Setter Property="HasShadow"        Value="False"/>
            </Style>

            <!-- ── Estilo botón primario ── -->
            <Style x:Key="PrimaryButton" TargetType="Button">
                <Setter Property="Background"       Value="{StaticResource PrimaryButtonGradient}"/>
                <Setter Property="TextColor"        Value="White"/>
                <Setter Property="CornerRadius"     Value="22"/>
                <Setter Property="HeightRequest"    Value="48"/>
                <Setter Property="FontAttributes"   Value="Bold"/>
                <Setter Property="FontFamily"       Value="OpenSansRegular"/>
                <Setter Property="FontSize"         Value="15"/>
            </Style>

            <!-- ── Estilo botón peligro (Cerrar sesión) ── -->
            <Style x:Key="DangerButton" TargetType="Button">
                <Setter Property="BackgroundColor"  Value="{StaticResource DangerGlass}"/>
                <Setter Property="BorderColor"      Value="{StaticResource DangerGlassBorder}"/>
                <Setter Property="BorderWidth"      Value="1"/>
                <Setter Property="TextColor"        Value="{StaticResource DangerText}"/>
                <Setter Property="CornerRadius"     Value="22"/>
                <Setter Property="HeightRequest"    Value="48"/>
                <Setter Property="FontAttributes"   Value="Bold"/>
                <Setter Property="FontFamily"       Value="OpenSansRegular"/>
                <Setter Property="FontSize"         Value="15"/>
            </Style>

            <!-- ── Estilo botón outline ── -->
            <Style x:Key="OutlineButton" TargetType="Button">
                <Setter Property="BackgroundColor"  Value="{StaticResource GlassHeaderBackground}"/>
                <Setter Property="BorderColor"      Value="{StaticResource GlassCardBorder}"/>
                <Setter Property="BorderWidth"      Value="1"/>
                <Setter Property="TextColor"        Value="White"/>
                <Setter Property="CornerRadius"     Value="22"/>
                <Setter Property="HeightRequest"    Value="44"/>
                <Setter Property="FontFamily"       Value="OpenSansRegular"/>
                <Setter Property="FontSize"         Value="14"/>
            </Style>

            <!-- ── Estilos globales por defecto ── -->
            <Style TargetType="Button">
                <Setter Property="FontFamily"       Value="OpenSansRegular"/>
                <Setter Property="FontSize"         Value="15"/>
                <Setter Property="CornerRadius"     Value="22"/>
                <Setter Property="HeightRequest"    Value="48"/>
                <Setter Property="TextColor"        Value="White"/>
                <Setter Property="Background"       Value="{StaticResource PrimaryButtonGradient}"/>
            </Style>

            <Style TargetType="Entry">
                <Setter Property="FontFamily"       Value="OpenSansRegular"/>
                <Setter Property="FontSize"         Value="15"/>
                <Setter Property="BackgroundColor"  Value="Transparent"/>
                <Setter Property="TextColor"        Value="White"/>
                <Setter Property="PlaceholderColor" Value="{StaticResource TextOnDarkSecondary}"/>
            </Style>

            <Style TargetType="Label">
                <Setter Property="FontFamily"       Value="OpenSansRegular"/>
                <Setter Property="TextColor"        Value="{StaticResource TextOnDark}"/>
            </Style>

        </ResourceDictionary>
    </Application.Resources>
</Application>
```

- [ ] **Step 2: Build to verify no errors**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add App.xaml
git commit -m "style: add glassmorphism color tokens and named styles"
```

---

## Task 2: AppShell.xaml — Shell & flyout redesign

**Files:**
- Modify: `AppShell.xaml`

- [ ] **Step 1: Replace AppShell.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:PettoV1.Views"
       xmlns:converters="clr-namespace:PettoV1.Converters"
       x:Class="PettoV1.AppShell"
       Shell.NavBarIsVisible="False"
       FlyoutBehavior="Flyout"
       FlyoutWidth="240"
       BackgroundColor="{StaticResource GradientMid}">

    <Shell.Resources>
        <converters:FlyoutTitleToColorConverter x:Key="TitleToColor"/>
    </Shell.Resources>

    <Shell.ItemTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="0.2*,0.8*"
                  HeightRequest="50"
                  Padding="8,0">
                <Image Grid.Column="0"
                       Source="{Binding Icon}"
                       WidthRequest="22" HeightRequest="22"
                       HorizontalOptions="Center"
                       VerticalOptions="Center"/>
                <Label Grid.Column="1"
                       Text="{Binding Title}"
                       TextColor="{StaticResource TextOnDark}"
                       FontSize="15"
                       VerticalOptions="Center"/>
            </Grid>
        </DataTemplate>
    </Shell.ItemTemplate>

    <!-- Encabezado del Flyout -->
    <Shell.FlyoutHeader>
        <Grid Padding="16,40,16,16">
            <Grid.Background>
                <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
                    <GradientStop Color="{StaticResource GradientStart}" Offset="0"/>
                    <GradientStop Color="{StaticResource GradientMid}"   Offset="1"/>
                </LinearGradientBrush>
            </Grid.Background>
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            <Ellipse Grid.Row="0"
                     WidthRequest="70" HeightRequest="70"
                     HorizontalOptions="Start"
                     Fill="{StaticResource GlassCardBackground}"
                     Stroke="{StaticResource GlassCardBorder}"
                     StrokeThickness="2"/>
            <Label Grid.Row="1"
                   Text="Usuario"
                   FontSize="16"
                   FontAttributes="Bold"
                   TextColor="{StaticResource TextOnDark}"
                   Margin="0,8,0,0"/>
        </Grid>
    </Shell.FlyoutHeader>

    <Shell.FlyoutFooter>
        <StackLayout Padding="16,8" BackgroundColor="{StaticResource GradientMid}">
            <BoxView HeightRequest="1" Color="{StaticResource GlassCardBorder}" Margin="0,0,0,8"/>
            <Button Text="Cerrar Sesión"
                    Style="{StaticResource DangerButton}"
                    Command="{Binding CerrarSesionCommand}"/>
        </StackLayout>
    </Shell.FlyoutFooter>

    <!-- Login: sin flyout, sin tabbar -->
    <ShellContent Route="Login"
                  ContentTemplate="{DataTemplate views:Login}"
                  Shell.TabBarIsVisible="False"
                  Shell.NavBarIsVisible="False"
                  FlyoutItemIsVisible="False"/>

    <!-- FlyoutItem principal: contiene los 4 tabs -->
    <FlyoutItem Title="Home" Icon="home_icon.png"
                Shell.TabBarForegroundColor="{StaticResource AccentCyan}"
                Shell.TabBarUnselectedColor="{StaticResource TextOnDarkSecondary}"
                Shell.TabBarBackgroundColor="{StaticResource TabBarBackground}">
        <Tab Title="Home" Icon="home_icon.png">
            <ShellContent Route="MainPage"
                          ContentTemplate="{DataTemplate views:MainPage}"
                          Shell.NavBarIsVisible="False"/>
        </Tab>
        <Tab Title="Chat" Icon="chat_icon.png">
            <ShellContent Route="Chat"
                          ContentTemplate="{DataTemplate views:Chat}"
                          Shell.NavBarIsVisible="False"/>
        </Tab>
        <Tab Title="Tareas" Icon="tasks_icon.png">
            <ShellContent Route="Tareas"
                          ContentTemplate="{DataTemplate views:Tareas}"
                          Shell.NavBarIsVisible="False"/>
        </Tab>
        <Tab Title="Gráficas" Icon="stats_icon.png">
            <ShellContent Route="Estadisticas"
                          ContentTemplate="{DataTemplate views:Estadisticas}"
                          Shell.NavBarIsVisible="False"/>
        </Tab>
    </FlyoutItem>

    <!-- FlyoutItems secundarios -->
    <FlyoutItem Title="Historial de Tareas" Icon="history_icon.png">
        <ShellContent Route="HistorialTareas"
                      ContentTemplate="{DataTemplate views:HistorialTareas}"
                      Shell.NavBarIsVisible="False"
                      Shell.TabBarIsVisible="False"/>
    </FlyoutItem>

    <FlyoutItem Title="Configuraciones" Icon="settings_icon.png">
        <ShellContent Route="Configuracion"
                      ContentTemplate="{DataTemplate views:Configuracion}"
                      Shell.NavBarIsVisible="False"
                      Shell.TabBarIsVisible="False"/>
    </FlyoutItem>

</Shell>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add AppShell.xaml
git commit -m "style: glassmorphism flyout menu and shell colors"
```

---

## Task 3: Views/Login.xaml — Login glassmorphism

**Files:**
- Modify: `Views/Login.xaml`

- [ ] **Step 1: Replace Views/Login.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="PettoV1.Views.Login"
             Title="Login">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="#FF0288D1" Offset="0.0"/>
            <GradientStop Color="#FF26C6DA" Offset="0.5"/>
            <GradientStop Color="#FFB2EBF2" Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <ScrollView>
        <VerticalStackLayout Spacing="0">

            <!-- Hero superior -->
            <VerticalStackLayout BackgroundColor="#990288D1"
                                 Padding="24,60,24,28"
                                 Spacing="4">
                <Label Text="🐾 Petto"
                       FontSize="32" FontAttributes="Bold"
                       HorizontalOptions="Center"
                       TextColor="White"/>
                <Label Text="Cuida a tu mascota"
                       FontSize="13"
                       HorizontalOptions="Center"
                       TextColor="{StaticResource TextOnDarkSecondary}"/>
            </VerticalStackLayout>

            <!-- Formulario glass -->
            <VerticalStackLayout Padding="20,24" Spacing="14">

                <Label Text="Iniciar sesión"
                       FontSize="22" FontAttributes="Bold"
                       TextColor="White"/>

                <!-- Correo -->
                <Label Text="CORREO" FontSize="10" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDarkSecondary}"
                       CharacterSpacing="1.5"/>
                <Border BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}"
                        StrokeThickness="1"
                        Padding="4,0">
                    <Border.StrokeShape>
                        <RoundRectangle CornerRadius="10"/>
                    </Border.StrokeShape>
                    <Border.Triggers>
                        <DataTrigger TargetType="Border"
                                     Binding="{Binding IsEmailValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Border"
                                     Binding="{Binding IsEmailValid}" Value="True">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                        </DataTrigger>
                    </Border.Triggers>
                    <Entry Placeholder="ejemplo@correo.com"
                           Keyboard="Email"
                           Text="{Binding Email}"
                           BackgroundColor="Transparent"/>
                </Border>
                <Label Text="Ingresa un correo electrónico válido"
                       TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label"
                                     Binding="{Binding IsEmailValid}" Value="False">
                            <Setter Property="IsVisible" Value="True"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label"
                                     Binding="{Binding IsEmailValid}" Value="True">
                            <Setter Property="IsVisible" Value="False"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>

                <!-- Contraseña -->
                <Label Text="CONTRASEÑA" FontSize="10" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDarkSecondary}"
                       CharacterSpacing="1.5"/>
                <Border BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}"
                        StrokeThickness="1"
                        Padding="4,0">
                    <Border.StrokeShape>
                        <RoundRectangle CornerRadius="10"/>
                    </Border.StrokeShape>
                    <Border.Triggers>
                        <DataTrigger TargetType="Border"
                                     Binding="{Binding IsPasswordValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Border"
                                     Binding="{Binding IsPasswordValid}" Value="True">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                        </DataTrigger>
                    </Border.Triggers>
                    <Entry Placeholder="Mínimo 6 caracteres"
                           IsPassword="True"
                           Text="{Binding Contrasena}"
                           BackgroundColor="Transparent"/>
                </Border>
                <Label Text="La contraseña debe tener al menos 6 caracteres"
                       TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label"
                                     Binding="{Binding IsPasswordValid}" Value="False">
                            <Setter Property="IsVisible" Value="True"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label"
                                     Binding="{Binding IsPasswordValid}" Value="True">
                            <Setter Property="IsVisible" Value="False"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>

                <!-- Botón Entrar -->
                <Button Text="Entrar"
                        Command="{Binding IniciarSesionCommand}"
                        Margin="0,8,0,0">
                    <Button.Triggers>
                        <DataTrigger TargetType="Button"
                                     Binding="{Binding IsFormValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="#80607D8B"/>
                            <Setter Property="IsEnabled"       Value="False"/>
                        </DataTrigger>
                    </Button.Triggers>
                </Button>

                <!-- Enlace Registro -->
                <Label HorizontalOptions="Center" Margin="0,4,0,0">
                    <Label.FormattedText>
                        <FormattedString>
                            <Span Text="¿No tienes cuenta? "
                                  TextColor="{StaticResource TextOnDarkSecondary}" FontSize="14"/>
                            <Span Text="Registrarse"
                                  TextColor="{StaticResource TextOnDark}" FontSize="14"
                                  FontAttributes="Bold"
                                  TextDecorations="Underline">
                                <Span.GestureRecognizers>
                                    <TapGestureRecognizer Command="{Binding IrARegistroCommand}"/>
                                </Span.GestureRecognizers>
                            </Span>
                        </FormattedString>
                    </Label.FormattedText>
                </Label>

            </VerticalStackLayout>
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Login.xaml
git commit -m "style: glassmorphism Login page"
```

---

## Task 4: Views/Registro.xaml — Registro glassmorphism

**Files:**
- Modify: `Views/Registro.xaml`

- [ ] **Step 1: Replace Views/Registro.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="PettoV1.Views.Registro"
             Title="Registro">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="#FF0288D1" Offset="0.0"/>
            <GradientStop Color="#FF26C6DA" Offset="0.5"/>
            <GradientStop Color="#FFB2EBF2" Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <ScrollView>
        <VerticalStackLayout Spacing="0">

            <!-- Hero superior -->
            <VerticalStackLayout BackgroundColor="#990288D1"
                                 Padding="24,50,24,20"
                                 Spacing="4">
                <Label Text="🐾 Crear cuenta"
                       FontSize="26" FontAttributes="Bold"
                       HorizontalOptions="Center"
                       TextColor="White"/>
                <Label Text="Únete a Petto"
                       FontSize="13"
                       HorizontalOptions="Center"
                       TextColor="{StaticResource TextOnDarkSecondary}"/>
            </VerticalStackLayout>

            <VerticalStackLayout Padding="20,20" Spacing="12">

                <!-- Correo -->
                <Label Text="CORREO" FontSize="10" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDarkSecondary}" CharacterSpacing="1.5"/>
                <Border BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}" StrokeThickness="1" Padding="4,0">
                    <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                    <Border.Triggers>
                        <DataTrigger TargetType="Border" Binding="{Binding IsEmailValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Border" Binding="{Binding IsEmailValid}" Value="True">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                        </DataTrigger>
                    </Border.Triggers>
                    <Entry Placeholder="ejemplo@correo.com" Keyboard="Email"
                           Text="{Binding Email}" BackgroundColor="Transparent"/>
                </Border>
                <Label Text="Ingresa un correo electrónico válido"
                       TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label" Binding="{Binding IsEmailValid}" Value="False">
                            <Setter Property="IsVisible" Value="True"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label" Binding="{Binding IsEmailValid}" Value="True">
                            <Setter Property="IsVisible" Value="False"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>

                <!-- Nombre de usuario -->
                <Label Text="USUARIO" FontSize="10" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDarkSecondary}" CharacterSpacing="1.5"/>
                <Border BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}" StrokeThickness="1" Padding="4,0">
                    <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                    <Border.Triggers>
                        <DataTrigger TargetType="Border" Binding="{Binding IsUsernameValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Border" Binding="{Binding IsUsernameValid}" Value="True">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                        </DataTrigger>
                    </Border.Triggers>
                    <Entry Placeholder="Mínimo 3 caracteres"
                           Text="{Binding NombreUsuario}" BackgroundColor="Transparent"/>
                </Border>
                <Label Text="El nombre debe tener al menos 3 caracteres"
                       TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label" Binding="{Binding IsUsernameValid}" Value="False">
                            <Setter Property="IsVisible" Value="True"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label" Binding="{Binding IsUsernameValid}" Value="True">
                            <Setter Property="IsVisible" Value="False"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>

                <!-- Contraseña -->
                <Label Text="CONTRASEÑA" FontSize="10" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDarkSecondary}" CharacterSpacing="1.5"/>
                <Border BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}" StrokeThickness="1" Padding="4,0">
                    <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                    <Border.Triggers>
                        <DataTrigger TargetType="Border" Binding="{Binding IsPasswordValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Border" Binding="{Binding IsPasswordValid}" Value="True">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                        </DataTrigger>
                    </Border.Triggers>
                    <Entry Placeholder="Mínimo 6 caracteres con un número"
                           IsPassword="True" Text="{Binding Contrasena}" BackgroundColor="Transparent"/>
                </Border>
                <Label Text="Mínimo 6 caracteres y al menos un número"
                       TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label" Binding="{Binding IsPasswordValid}" Value="False">
                            <Setter Property="IsVisible" Value="True"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label" Binding="{Binding IsPasswordValid}" Value="True">
                            <Setter Property="IsVisible" Value="False"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>

                <!-- Confirmar contraseña -->
                <Label Text="CONFIRMAR" FontSize="10" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDarkSecondary}" CharacterSpacing="1.5"/>
                <Border BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}" StrokeThickness="1" Padding="4,0">
                    <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                    <Border.Triggers>
                        <DataTrigger TargetType="Border" Binding="{Binding IsConfirmPasswordValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Border" Binding="{Binding IsConfirmPasswordValid}" Value="True">
                            <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                        </DataTrigger>
                    </Border.Triggers>
                    <Entry Placeholder="Repite tu contraseña"
                           IsPassword="True" Text="{Binding ConfirmarContrasena}" BackgroundColor="Transparent"/>
                </Border>
                <Label Text="Las contraseñas no coinciden"
                       TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label" Binding="{Binding IsConfirmPasswordValid}" Value="False">
                            <Setter Property="IsVisible" Value="True"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label" Binding="{Binding IsConfirmPasswordValid}" Value="True">
                            <Setter Property="IsVisible" Value="False"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>

                <!-- Botón Registrarse -->
                <Button Text="Registrarse"
                        Command="{Binding RegistrarseCommand}"
                        Margin="0,8,0,0">
                    <Button.Triggers>
                        <DataTrigger TargetType="Button"
                                     Binding="{Binding IsFormValid}" Value="False">
                            <Setter Property="BackgroundColor" Value="#80607D8B"/>
                            <Setter Property="IsEnabled"       Value="False"/>
                        </DataTrigger>
                    </Button.Triggers>
                </Button>

                <!-- Enlace Login -->
                <Label HorizontalOptions="Center" Margin="0,4,0,0">
                    <Label.FormattedText>
                        <FormattedString>
                            <Span Text="¿Ya tienes cuenta? "
                                  TextColor="{StaticResource TextOnDarkSecondary}" FontSize="14"/>
                            <Span Text="Ingresar"
                                  TextColor="{StaticResource TextOnDark}" FontSize="14"
                                  FontAttributes="Bold" TextDecorations="Underline">
                                <Span.GestureRecognizers>
                                    <TapGestureRecognizer Command="{Binding IrALoginCommand}"/>
                                </Span.GestureRecognizers>
                            </Span>
                        </FormattedString>
                    </Label.FormattedText>
                </Label>

            </VerticalStackLayout>
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Registro.xaml
git commit -m "style: glassmorphism Registro page"
```

---

## Task 5: Views/MainPage.xaml — Home glassmorphism

**Files:**
- Modify: `Views/MainPage.xaml`

- [ ] **Step 1: Replace Views/MainPage.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:PettoV1.ViewModels"
             x:Class="PettoV1.Views.MainPage"
             x:DataType="vm:MainViewModel"
             Title="Home">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
            <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
            <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <Grid RowDefinitions="Auto,*" RowSpacing="0">

        <!-- ── Header glass ── -->
        <Grid Grid.Row="0" ColumnDefinitions="Auto,*,Auto"
              Padding="12,44,12,10"
              BackgroundColor="{StaticResource GlassHeaderBackground}">
            <ImageButton Grid.Column="0"
                         Source="menu_icon.png"
                         WidthRequest="36" HeightRequest="36"
                         BackgroundColor="Transparent"
                         Command="{Binding AbrirMenuCommand}"/>
            <Label Grid.Column="1"
                   Text="{Binding FechaHora}"
                   HorizontalOptions="Center"
                   VerticalOptions="Center"
                   FontSize="13"
                   TextColor="{StaticResource TextOnDarkSecondary}"/>
            <ImageButton Grid.Column="2"
                         Source="user_icon.png"
                         WidthRequest="44" HeightRequest="44"
                         BackgroundColor="{StaticResource GlassCardBackground}"
                         CornerRadius="22"
                         Command="{Binding IrAPerfilCommand}"/>
        </Grid>

        <!-- ── Contenido principal ── -->
        <ScrollView Grid.Row="1">
            <VerticalStackLayout Padding="16" Spacing="20">

                <!-- Card mascota glass -->
                <Frame Style="{StaticResource GlassCard}"
                       Padding="20,24">
                    <VerticalStackLayout Spacing="12" HorizontalOptions="Center">

                        <!-- Avatar circular con gradiente -->
                        <Grid WidthRequest="120" HeightRequest="120"
                              HorizontalOptions="Center">
                            <Ellipse WidthRequest="120" HeightRequest="120"
                                     Stroke="{StaticResource GlassCardBorder}"
                                     StrokeThickness="2">
                                <Ellipse.Fill>
                                    <LinearGradientBrush StartPoint="0,0" EndPoint="1,1">
                                        <GradientStop Color="{StaticResource AccentCyan}" Offset="0"/>
                                        <GradientStop Color="{StaticResource AccentBlue}" Offset="1"/>
                                    </LinearGradientBrush>
                                </Ellipse.Fill>
                            </Ellipse>
                        </Grid>

                        <!-- Puntos de cuidado -->
                        <Label FontSize="15" TextColor="{StaticResource TextOnDarkSecondary}"
                               HorizontalOptions="Center">
                            <Label.FormattedText>
                                <FormattedString>
                                    <Span Text="Puntos de cuidado: "
                                          TextColor="{StaticResource TextOnDarkSecondary}"/>
                                    <Span Text="{Binding PuntosDesCuidado}"
                                          FontSize="20" FontAttributes="Bold"
                                          TextColor="{StaticResource TextOnDark}"/>
                                </FormattedString>
                            </Label.FormattedText>
                        </Label>

                    </VerticalStackLayout>
                </Frame>

                <!-- Botones de interacción — translúcidos -->
                <HorizontalStackLayout HorizontalOptions="Center" Spacing="12">
                    <Ellipse WidthRequest="45" HeightRequest="45"
                             Fill="#4D29B6F2" Stroke="#8029B6F2" StrokeThickness="1.5">
                        <Ellipse.GestureRecognizers>
                            <TapGestureRecognizer/>
                        </Ellipse.GestureRecognizers>
                    </Ellipse>
                    <Ellipse WidthRequest="45" HeightRequest="45"
                             Fill="#4D66BB6A" Stroke="#8066BB6A" StrokeThickness="1.5">
                        <Ellipse.GestureRecognizers>
                            <TapGestureRecognizer/>
                        </Ellipse.GestureRecognizers>
                    </Ellipse>
                    <Ellipse WidthRequest="45" HeightRequest="45"
                             Fill="#4DFFB74D" Stroke="#80FFB74D" StrokeThickness="1.5">
                        <Ellipse.GestureRecognizers>
                            <TapGestureRecognizer/>
                        </Ellipse.GestureRecognizers>
                    </Ellipse>
                    <Ellipse WidthRequest="45" HeightRequest="45"
                             Fill="#4DCE93D8" Stroke="#80CE93D8" StrokeThickness="1.5">
                        <Ellipse.GestureRecognizers>
                            <TapGestureRecognizer/>
                        </Ellipse.GestureRecognizers>
                    </Ellipse>
                    <Ellipse WidthRequest="45" HeightRequest="45"
                             Fill="#4DEF9A9A" Stroke="#80EF9A9A" StrokeThickness="1.5">
                        <Ellipse.GestureRecognizers>
                            <TapGestureRecognizer/>
                        </Ellipse.GestureRecognizers>
                    </Ellipse>
                </HorizontalStackLayout>

            </VerticalStackLayout>
        </ScrollView>

    </Grid>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/MainPage.xaml
git commit -m "style: glassmorphism MainPage"
```

---

## Task 6: Views/Tareas.xaml — Tareas glassmorphism

**Files:**
- Modify: `Views/Tareas.xaml`

- [ ] **Step 1: Replace Views/Tareas.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="PettoV1.Views.Tareas"
             Title="Tareas">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
            <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
            <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <Grid RowDefinitions="Auto,*" RowSpacing="0">

        <!-- ── Header glass ── -->
        <Grid Grid.Row="0" ColumnDefinitions="Auto,*,Auto"
              Padding="12,44,12,10"
              BackgroundColor="{StaticResource GlassHeaderBackground}">
            <ImageButton Grid.Column="0"
                         Source="menu_icon.png"
                         WidthRequest="36" HeightRequest="36"
                         BackgroundColor="Transparent"
                         Command="{Binding AbrirMenuCommand}"/>
            <Label Grid.Column="1"
                   Text="{Binding FechaHora}"
                   HorizontalOptions="Center"
                   VerticalOptions="Center"
                   FontSize="13"
                   TextColor="{StaticResource TextOnDarkSecondary}"/>
            <ImageButton Grid.Column="2"
                         Source="user_icon.png"
                         WidthRequest="44" HeightRequest="44"
                         BackgroundColor="{StaticResource GlassCardBackground}"
                         CornerRadius="22"
                         Command="{Binding IrAPerfilCommand}"/>
        </Grid>

        <!-- ── Contenido ── -->
        <ScrollView Grid.Row="1">
            <VerticalStackLayout Padding="16" Spacing="16">

                <!-- ── Tareas próximas ── -->
                <Label Text="TAREAS PRÓXIMAS"
                       FontSize="10" FontAttributes="Bold"
                       CharacterSpacing="1.5"
                       TextColor="{StaticResource AccentCyan}"/>

                <Frame Style="{StaticResource GlassCard}" Padding="12,8">
                    <CollectionView ItemsSource="{Binding TareasProximas}">
                        <CollectionView.EmptyView>
                            <Label Text="No hay tareas próximas"
                                   FontSize="14"
                                   TextColor="{StaticResource TextOnDarkSecondary}"
                                   HorizontalOptions="Center"
                                   Margin="0,8"/>
                        </CollectionView.EmptyView>
                        <CollectionView.ItemTemplate>
                            <DataTemplate>
                                <Grid ColumnDefinitions="Auto,*" Padding="0,8">
                                    <BoxView Grid.Column="0"
                                             WidthRequest="8" HeightRequest="8"
                                             BackgroundColor="{StaticResource AccentCyan}"
                                             CornerRadius="4"
                                             VerticalOptions="Center"
                                             Margin="0,0,10,0"/>
                                    <Label Grid.Column="1"
                                           Text="{Binding Titulo}"
                                           FontSize="15"
                                           TextColor="{StaticResource TextOnDark}"
                                           VerticalOptions="Center"/>
                                </Grid>
                            </DataTemplate>
                        </CollectionView.ItemTemplate>
                    </CollectionView>
                </Frame>

                <!-- Separador acento -->
                <BoxView HeightRequest="1.5" Margin="0,4">
                    <BoxView.Background>
                        <LinearGradientBrush StartPoint="0,0" EndPoint="1,0">
                            <GradientStop Color="{StaticResource AccentCyan}" Offset="0"/>
                            <GradientStop Color="Transparent" Offset="1"/>
                        </LinearGradientBrush>
                    </BoxView.Background>
                </BoxView>

                <!-- ── Categorías + botón agregar ── -->
                <Grid ColumnDefinitions="*,Auto">
                    <Label Grid.Column="0"
                           Text="CATEGORÍAS"
                           FontSize="10" FontAttributes="Bold"
                           CharacterSpacing="1.5"
                           TextColor="{StaticResource AccentCyan}"
                           VerticalOptions="Center"/>
                    <Frame Grid.Column="1"
                           BackgroundColor="{StaticResource GlassCardBackground}"
                           BorderColor="{StaticResource GlassCardBorder}"
                           CornerRadius="20" Padding="0"
                           WidthRequest="40" HeightRequest="40"
                           HasShadow="False">
                        <Label Text="+"
                               FontSize="26"
                               TextColor="{StaticResource AccentCyan}"
                               HorizontalOptions="Center"
                               VerticalOptions="Center"
                               Margin="0,-2,0,0">
                            <Label.GestureRecognizers>
                                <TapGestureRecognizer Command="{Binding AgregarCategoriaCommand}"/>
                            </Label.GestureRecognizers>
                        </Label>
                    </Frame>
                </Grid>

                <!-- Grid de categorías 2 columnas -->
                <CollectionView ItemsSource="{Binding Categorias}">
                    <CollectionView.EmptyView>
                        <VerticalStackLayout HorizontalOptions="Center" Spacing="8" Margin="0,20">
                            <Label Text="Sin categorías aún"
                                   FontSize="14"
                                   TextColor="{StaticResource TextOnDarkSecondary}"
                                   HorizontalOptions="Center"/>
                            <Label Text="Toca el botón + para agregar una"
                                   FontSize="12"
                                   TextColor="{StaticResource TextOnDarkSecondary}"
                                   HorizontalOptions="Center"/>
                        </VerticalStackLayout>
                    </CollectionView.EmptyView>
                    <CollectionView.ItemsLayout>
                        <GridItemsLayout Orientation="Vertical"
                                         Span="2"
                                         HorizontalItemSpacing="10"
                                         VerticalItemSpacing="10"/>
                    </CollectionView.ItemsLayout>
                    <CollectionView.ItemTemplate>
                        <DataTemplate>
                            <Frame BackgroundColor="{StaticResource GlassCardBackground}"
                                   BorderColor="{StaticResource GlassCardBorder}"
                                   CornerRadius="14"
                                   Padding="12"
                                   HasShadow="False"
                                   HeightRequest="80">
                                <Frame.GestureRecognizers>
                                    <TapGestureRecognizer
                                        Command="{Binding Source={RelativeSource AncestorType={x:Type ContentPage}},
                                                          Path=BindingContext.VerCategoriaCommand}"
                                        CommandParameter="{Binding .}"/>
                                </Frame.GestureRecognizers>
                                <Grid RowDefinitions="*,Auto">
                                    <Label Grid.Row="0"
                                           Text="{Binding Nombre}"
                                           FontSize="15"
                                           FontAttributes="Bold"
                                           TextColor="{StaticResource TextOnDark}"
                                           HorizontalOptions="Center"
                                           VerticalOptions="Center"
                                           HorizontalTextAlignment="Center"/>
                                    <!-- Línea decorativa inferior con gradiente acento -->
                                    <BoxView Grid.Row="1"
                                             HeightRequest="2.5"
                                             CornerRadius="2"
                                             Margin="0,4,0,0">
                                        <BoxView.Background>
                                            <LinearGradientBrush StartPoint="0,0" EndPoint="1,0">
                                                <GradientStop Color="{StaticResource AccentBlue}"  Offset="0"/>
                                                <GradientStop Color="{StaticResource AccentCyan}"  Offset="1"/>
                                            </LinearGradientBrush>
                                        </BoxView.Background>
                                    </BoxView>
                                </Grid>
                            </Frame>
                        </DataTemplate>
                    </CollectionView.ItemTemplate>
                </CollectionView>

            </VerticalStackLayout>
        </ScrollView>

    </Grid>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Tareas.xaml
git commit -m "style: glassmorphism Tareas page"
```

---

## Task 7: Views/Chat.xaml — Chat glassmorphism

**Files:**
- Modify: `Views/Chat.xaml`

- [ ] **Step 1: Replace Views/Chat.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:PettoV1.ViewModels"
             xmlns:models="clr-namespace:SharedResources.Models;assembly=SharedResources"
             x:Class="PettoV1.Views.Chat"
             x:DataType="vm:ChatViewModel"
             Title="Chat">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
            <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
            <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <Grid RowDefinitions="Auto,*,Auto" RowSpacing="0">

        <!-- ── Header glass ── -->
        <Grid Grid.Row="0" ColumnDefinitions="Auto,*,Auto"
              Padding="12,44,12,10"
              BackgroundColor="{StaticResource GlassHeaderBackground}">
            <ImageButton Grid.Column="0" Source="menu_icon.png"
                         WidthRequest="36" HeightRequest="36"
                         BackgroundColor="Transparent"
                         Command="{Binding AbrirMenuCommand}"/>
            <Label Grid.Column="1"
                   Text="{Binding FechaHora}"
                   HorizontalOptions="Center" VerticalOptions="Center"
                   FontSize="13" TextColor="{StaticResource TextOnDarkSecondary}"/>
            <ImageButton Grid.Column="2" Source="user_icon.png"
                         WidthRequest="44" HeightRequest="44"
                         BackgroundColor="{StaticResource GlassCardBackground}"
                         CornerRadius="22"
                         Command="{Binding IrAPerfilCommand}"/>
        </Grid>

        <!-- ── Lista de mensajes ── -->
        <CollectionView Grid.Row="1"
                        x:Name="MensajesCollection"
                        ItemsSource="{Binding Mensajes}"
                        Margin="0,4">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="models:MensajeModel">
                    <Grid Padding="12,6">
                        <!-- Burbuja usuario (derecha) — gradiente -->
                        <Border HorizontalOptions="End"
                                MaximumWidthRequest="280"
                                StrokeThickness="0"
                                Padding="12,8"
                                IsVisible="{Binding EsRespuestaIA, Converter={StaticResource InverseBoolConverter}}">
                            <Border.Background>
                                <LinearGradientBrush StartPoint="0,0" EndPoint="1,0">
                                    <GradientStop Color="{StaticResource AccentBlue}" Offset="0"/>
                                    <GradientStop Color="{StaticResource AccentCyan}" Offset="1"/>
                                </LinearGradientBrush>
                            </Border.Background>
                            <Border.StrokeShape>
                                <RoundRectangle CornerRadius="14,14,4,14"/>
                            </Border.StrokeShape>
                            <Label Text="{Binding Contenido}"
                                   TextColor="White"
                                   FontSize="14"/>
                        </Border>
                        <!-- Burbuja IA (izquierda) — glass -->
                        <Border HorizontalOptions="Start"
                                MaximumWidthRequest="280"
                                BackgroundColor="{StaticResource GlassCardBackground}"
                                Stroke="{StaticResource GlassCardBorder}"
                                StrokeThickness="1"
                                Padding="12,8"
                                IsVisible="{Binding EsRespuestaIA}">
                            <Border.StrokeShape>
                                <RoundRectangle CornerRadius="14,14,14,4"/>
                            </Border.StrokeShape>
                            <Label Text="{Binding Contenido}"
                                   TextColor="{StaticResource TextOnDark}"
                                   FontSize="14"/>
                        </Border>
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

        <!-- ── Área de entrada de texto glass ── -->
        <Frame Grid.Row="2"
               Style="{StaticResource GlassCard}"
               CornerRadius="0"
               Padding="12,8">
            <Grid ColumnDefinitions="*,Auto" ColumnSpacing="8">
                <Border Grid.Column="0"
                        BackgroundColor="{StaticResource GlassInputBackground}"
                        Stroke="{StaticResource GlassCardBorder}"
                        StrokeThickness="1"
                        Padding="4,0">
                    <Border.StrokeShape>
                        <RoundRectangle CornerRadius="20"/>
                    </Border.StrokeShape>
                    <Entry Placeholder="Escribe un mensaje..."
                           Text="{Binding MensajeActual}"
                           ReturnCommand="{Binding EnviarMensajeCommand}"
                           BackgroundColor="Transparent"/>
                </Border>
                <Button Grid.Column="1"
                        Text="➤"
                        WidthRequest="44" HeightRequest="44"
                        CornerRadius="22"
                        Padding="0"
                        IsEnabled="{Binding PuedeEnviar}"
                        Command="{Binding EnviarMensajeCommand}">
                    <Button.Triggers>
                        <DataTrigger TargetType="Button"
                                     Binding="{Binding PuedeEnviar}" Value="False">
                            <Setter Property="BackgroundColor" Value="#80607D8B"/>
                        </DataTrigger>
                    </Button.Triggers>
                </Button>
            </Grid>
        </Frame>

    </Grid>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Chat.xaml
git commit -m "style: glassmorphism Chat page with gradient user bubbles"
```

---

## Task 8: Views/Estadisticas.xaml — Estadísticas glassmorphism

**Files:**
- Modify: `Views/Estadisticas.xaml`

- [ ] **Step 1: Replace Views/Estadisticas.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:PettoV1.ViewModels"
             x:Class="PettoV1.Views.Estadisticas"
             x:DataType="vm:EstadisticasViewModel"
             Title="Estadísticas">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
            <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
            <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <Grid RowDefinitions="Auto,*" RowSpacing="0">

        <!-- ── Header glass ── -->
        <Grid Grid.Row="0" ColumnDefinitions="Auto,*,Auto"
              Padding="12,44,12,10"
              BackgroundColor="{StaticResource GlassHeaderBackground}">
            <ImageButton Grid.Column="0" Source="menu_icon.png"
                         WidthRequest="36" HeightRequest="36"
                         BackgroundColor="Transparent"
                         Command="{Binding AbrirMenuCommand}"/>
            <Label Grid.Column="1"
                   Text="{Binding FechaHora}"
                   HorizontalOptions="Center" VerticalOptions="Center"
                   FontSize="13" TextColor="{StaticResource TextOnDarkSecondary}"/>
            <ImageButton Grid.Column="2" Source="user_icon.png"
                         WidthRequest="44" HeightRequest="44"
                         BackgroundColor="{StaticResource GlassCardBackground}"
                         CornerRadius="22"
                         Command="{Binding IrAPerfilCommand}"/>
        </Grid>

        <!-- ── Contenido ── -->
        <ScrollView Grid.Row="1">
            <VerticalStackLayout Padding="20" Spacing="20">

                <Label Text="Estadísticas"
                       FontSize="24" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDark}"
                       HorizontalOptions="Center"/>

                <!-- Gráfica placeholder glass -->
                <Frame Style="{StaticResource GlassCard}"
                       HeightRequest="220">
                    <VerticalStackLayout VerticalOptions="Center" Spacing="8">
                        <Label Text="📊" FontSize="48" HorizontalOptions="Center"/>
                        <Label Text="Gráfica de tareas"
                               FontSize="16"
                               TextColor="{StaticResource TextOnDarkSecondary}"
                               HorizontalOptions="Center"/>
                        <Button Text="Mostrar estadísticas"
                                HorizontalOptions="Center"
                                Command="{Binding ActualizarEstadisticasCommand}"/>
                    </VerticalStackLayout>
                </Frame>

                <!-- Tarjetas de resumen -->
                <Grid ColumnDefinitions="*,*" ColumnSpacing="12" RowSpacing="12">

                    <!-- Total -->
                    <Frame Grid.Column="0"
                           BackgroundColor="{StaticResource GlassCardBackground}"
                           BorderColor="{StaticResource GlassCardBorder}"
                           CornerRadius="14" Padding="16" HasShadow="False">
                        <VerticalStackLayout Spacing="4">
                            <Label Text="Total" FontSize="13"
                                   TextColor="{StaticResource TextOnDarkSecondary}"/>
                            <Label Text="{Binding TotalTareas}"
                                   FontSize="32" FontAttributes="Bold"
                                   TextColor="{StaticResource TextOnDark}"/>
                            <Label Text="tareas" FontSize="12"
                                   TextColor="{StaticResource TextOnDarkSecondary}"/>
                        </VerticalStackLayout>
                    </Frame>

                    <!-- Completadas — tint cyan -->
                    <Frame Grid.Column="1"
                           BackgroundColor="#2626C6DA"
                           BorderColor="#5926C6DA"
                           CornerRadius="14" Padding="16" HasShadow="False">
                        <VerticalStackLayout Spacing="4">
                            <Label Text="Completadas" FontSize="13"
                                   TextColor="{StaticResource AccentCyan}"/>
                            <Label Text="{Binding TareasCompletadas}"
                                   FontSize="32" FontAttributes="Bold"
                                   TextColor="{StaticResource AccentCyan}"/>
                            <Label Text="tareas" FontSize="12"
                                   TextColor="{StaticResource AccentCyan}"/>
                        </VerticalStackLayout>
                    </Frame>

                    <!-- Pendientes — tint naranja -->
                    <Frame Grid.Row="1" Grid.Column="0"
                           BackgroundColor="#26FFA726"
                           BorderColor="#59FFA726"
                           CornerRadius="14" Padding="16" HasShadow="False">
                        <VerticalStackLayout Spacing="4">
                            <Label Text="Pendientes" FontSize="13"
                                   TextColor="#FFFFCC80"/>
                            <Label Text="{Binding TareasIncompletas}"
                                   FontSize="32" FontAttributes="Bold"
                                   TextColor="#FFFFCC80"/>
                            <Label Text="tareas" FontSize="12"
                                   TextColor="#FFFFCC80"/>
                        </VerticalStackLayout>
                    </Frame>

                    <!-- Progreso — tint púrpura -->
                    <Frame Grid.Row="1" Grid.Column="1"
                           BackgroundColor="#26AB47BC"
                           BorderColor="#59AB47BC"
                           CornerRadius="14" Padding="16" HasShadow="False">
                        <VerticalStackLayout Spacing="4">
                            <Label Text="Progreso" FontSize="13"
                                   TextColor="#FFCE93D8"/>
                            <Label Text="{Binding PorcentajeCompletadas, StringFormat='{0}%'}"
                                   FontSize="32" FontAttributes="Bold"
                                   TextColor="#FFCE93D8"/>
                            <Label Text="completado" FontSize="12"
                                   TextColor="#FFCE93D8"/>
                        </VerticalStackLayout>
                    </Frame>

                </Grid>

                <!-- Filtro de fecha -->
                <Label Text="Selecciona una fecha"
                       FontSize="16" FontAttributes="Bold"
                       TextColor="{StaticResource TextOnDark}"/>
                <Frame Style="{StaticResource GlassCard}" Padding="12,4">
                    <DatePicker Date="{Binding FechaSeleccionada}"
                                Format="dd/MM/yyyy"
                                TextColor="{StaticResource TextOnDark}"
                                BackgroundColor="Transparent"/>
                </Frame>

            </VerticalStackLayout>
        </ScrollView>

    </Grid>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Estadisticas.xaml
git commit -m "style: glassmorphism Estadisticas page"
```

---

## Task 9: Views/Perfil.xaml — Perfil glassmorphism

**Files:**
- Modify: `Views/Perfil.xaml`

- [ ] **Step 1: Replace Views/Perfil.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:PettoV1.ViewModels"
             x:Class="PettoV1.Views.Perfil"
             x:DataType="vm:PerfilViewModel"
             Title="Perfil">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
            <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
            <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <Grid RowDefinitions="Auto,*" RowSpacing="0">

        <!-- ── Header glass ── -->
        <Grid Grid.Row="0" ColumnDefinitions="Auto,*,Auto"
              Padding="12,44,12,10"
              BackgroundColor="{StaticResource GlassHeaderBackground}">
            <ImageButton Grid.Column="0" Source="arrow_left_icon.png"
                         WidthRequest="40" HeightRequest="40"
                         BackgroundColor="Transparent"
                         Command="{Binding RegresarCommand}"/>
            <Label Grid.Column="1"
                   Text="Perfil"
                   HorizontalOptions="Center" VerticalOptions="Center"
                   FontSize="20" FontAttributes="Bold"
                   TextColor="{StaticResource TextOnDark}"/>
            <ImageButton Grid.Column="2" Source="settings_icon.png"
                         WidthRequest="36" HeightRequest="36"
                         BackgroundColor="Transparent"
                         Command="{Binding IrAConfiguracionCommand}"/>
        </Grid>

        <!-- ── Contenido ── -->
        <ScrollView Grid.Row="1">
            <VerticalStackLayout Padding="20" Spacing="20">

                <!-- Avatar glass card -->
                <Frame Style="{StaticResource GlassCard}" Padding="20,24">
                    <VerticalStackLayout Spacing="10" HorizontalOptions="Center">
                        <Grid HorizontalOptions="Center"
                              WidthRequest="100" HeightRequest="100">
                            <Ellipse WidthRequest="100" HeightRequest="100"
                                     Stroke="{StaticResource GlassCardBorder}"
                                     StrokeThickness="2.5">
                                <Ellipse.Fill>
                                    <LinearGradientBrush StartPoint="0,0" EndPoint="1,1">
                                        <GradientStop Color="{StaticResource AccentCyan}" Offset="0"/>
                                        <GradientStop Color="{StaticResource AccentBlue}" Offset="1"/>
                                    </LinearGradientBrush>
                                </Ellipse.Fill>
                            </Ellipse>
                            <Image Source="user_large_icon.png"
                                   WidthRequest="64" HeightRequest="64"
                                   HorizontalOptions="Center"
                                   VerticalOptions="Center"/>
                            <ImageButton Source="edit_icon.png"
                                         WidthRequest="26" HeightRequest="26"
                                         BackgroundColor="{StaticResource AccentCyan}"
                                         CornerRadius="13"
                                         HorizontalOptions="End"
                                         VerticalOptions="End"/>
                        </Grid>
                        <Label Text="{Binding NombreUsuario}"
                               FontSize="20" FontAttributes="Bold"
                               TextColor="{StaticResource TextOnDark}"
                               HorizontalOptions="Center"/>
                    </VerticalStackLayout>
                </Frame>

                <!-- Info glass card -->
                <Frame Style="{StaticResource GlassCard}">
                    <VerticalStackLayout Spacing="0">

                        <!-- Email -->
                        <VerticalStackLayout Spacing="3" Padding="0,8">
                            <Label Text="EMAIL" FontSize="10" FontAttributes="Bold"
                                   CharacterSpacing="1.5"
                                   TextColor="{StaticResource TextOnDarkSecondary}"/>
                            <Label Text="{Binding Email}"
                                   FontSize="15" TextColor="{StaticResource TextOnDark}"/>
                        </VerticalStackLayout>

                        <BoxView HeightRequest="1" BackgroundColor="{StaticResource GlassHeaderBackground}" Margin="0,2"/>

                        <!-- Contraseña -->
                        <VerticalStackLayout Spacing="3" Padding="0,8">
                            <Label Text="CONTRASEÑA" FontSize="10" FontAttributes="Bold"
                                   CharacterSpacing="1.5"
                                   TextColor="{StaticResource TextOnDarkSecondary}"/>
                            <Label Text="••••••••"
                                   FontSize="15" TextColor="{StaticResource TextOnDark}"/>
                        </VerticalStackLayout>

                        <BoxView HeightRequest="1" BackgroundColor="{StaticResource GlassHeaderBackground}" Margin="0,2"/>

                        <!-- Teléfono editable -->
                        <VerticalStackLayout Spacing="3" Padding="0,8">
                            <Label Text="TELÉFONO" FontSize="10" FontAttributes="Bold"
                                   CharacterSpacing="1.5"
                                   TextColor="{StaticResource TextOnDarkSecondary}"/>
                            <Border BackgroundColor="{StaticResource GlassInputBackground}"
                                    Stroke="{StaticResource GlassCardBorder}"
                                    StrokeThickness="1"
                                    Padding="4,0">
                                <Border.StrokeShape>
                                    <RoundRectangle CornerRadius="10"/>
                                </Border.StrokeShape>
                                <Entry Text="{Binding Telefono}"
                                       Placeholder="Agrega tu teléfono"
                                       Keyboard="Telephone"
                                       BackgroundColor="Transparent"/>
                            </Border>
                        </VerticalStackLayout>

                    </VerticalStackLayout>
                </Frame>

                <!-- Botón Guardar -->
                <Button Text="Guardar cambios"
                        Command="{Binding GuardarPerfilCommand}"
                        Margin="0,4,0,0"/>

            </VerticalStackLayout>
        </ScrollView>

    </Grid>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Perfil.xaml
git commit -m "style: glassmorphism Perfil page"
```

---

## Task 10: Views/Configuracion.xaml — Configuración glassmorphism

**Files:**
- Modify: `Views/Configuracion.xaml`

- [ ] **Step 1: Replace Views/Configuracion.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:PettoV1.ViewModels"
             x:Class="PettoV1.Views.Configuracion"
             x:DataType="vm:ConfiguracionViewModel"
             Title="Configuraciones">

    <ContentPage.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="{StaticResource GradientStart}" Offset="0.0"/>
            <GradientStop Color="{StaticResource GradientMid}"   Offset="0.4"/>
            <GradientStop Color="{StaticResource GradientEnd}"   Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <Grid RowDefinitions="Auto,*" RowSpacing="0">

        <!-- ── Header glass ── -->
        <Grid Grid.Row="0" ColumnDefinitions="Auto,*,Auto"
              Padding="12,44,12,10"
              BackgroundColor="{StaticResource GlassHeaderBackground}">
            <ImageButton Grid.Column="0" Source="arrow_left_icon.png"
                         WidthRequest="40" HeightRequest="40"
                         BackgroundColor="Transparent"
                         Command="{Binding RegresarCommand}"/>
            <Label Grid.Column="1"
                   Text="Configuraciones"
                   HorizontalOptions="Center" VerticalOptions="Center"
                   FontSize="20" FontAttributes="Bold"
                   TextColor="{StaticResource TextOnDark}"/>
        </Grid>

        <!-- ── Contenido ── -->
        <ScrollView Grid.Row="1">
            <VerticalStackLayout Padding="20" Spacing="20">

                <!-- ── Sección Idioma ── -->
                <Label Text="IDIOMA" FontSize="10" FontAttributes="Bold"
                       CharacterSpacing="1.5" TextColor="{StaticResource AccentCyan}"/>

                <Frame Style="{StaticResource GlassCard}">
                    <VerticalStackLayout Spacing="10">
                        <Grid ColumnDefinitions="*,Auto">
                            <Label Grid.Column="0"
                                   Text="Idioma"
                                   FontSize="15" TextColor="{StaticResource TextOnDark}"
                                   VerticalOptions="Center"/>
                            <ImageButton Grid.Column="1"
                                         Source="arrow_right_icon.png"
                                         WidthRequest="28" HeightRequest="28"
                                         BackgroundColor="Transparent"/>
                            <Grid.GestureRecognizers>
                                <TapGestureRecognizer Command="{Binding GuardarIdiomaCommand}"/>
                            </Grid.GestureRecognizers>
                        </Grid>
                        <Border BackgroundColor="{StaticResource GlassInputBackground}"
                                Stroke="{StaticResource GlassCardBorder}"
                                StrokeThickness="1"
                                Padding="4,0">
                            <Border.StrokeShape>
                                <RoundRectangle CornerRadius="10"/>
                            </Border.StrokeShape>
                            <Picker ItemsSource="{Binding Idiomas}"
                                    SelectedItem="{Binding IdiomaSeleccionado}"
                                    TextColor="{StaticResource TextOnDark}"
                                    BackgroundColor="Transparent"/>
                        </Border>
                        <Button Text="Guardar idioma"
                                Style="{StaticResource OutlineButton}"
                                Command="{Binding GuardarIdiomaCommand}"/>
                    </VerticalStackLayout>
                </Frame>

                <!-- ── Sección Cambiar contraseña ── -->
                <Label Text="CAMBIAR CONTRASEÑA" FontSize="10" FontAttributes="Bold"
                       CharacterSpacing="1.5" TextColor="{StaticResource AccentCyan}"/>

                <Frame Style="{StaticResource GlassCard}">
                    <VerticalStackLayout Spacing="12">

                        <!-- Contraseña actual -->
                        <Label Text="Contraseña actual" FontSize="13"
                               TextColor="{StaticResource TextOnDarkSecondary}"/>
                        <Border BackgroundColor="{StaticResource GlassInputBackground}"
                                Stroke="{StaticResource GlassCardBorder}"
                                StrokeThickness="1" Padding="4,0">
                            <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                            <Border.Triggers>
                                <DataTrigger TargetType="Border"
                                             Binding="{Binding IsContrasenaActualValida}" Value="False">
                                    <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                                </DataTrigger>
                                <DataTrigger TargetType="Border"
                                             Binding="{Binding IsContrasenaActualValida}" Value="True">
                                    <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                                </DataTrigger>
                            </Border.Triggers>
                            <Entry Placeholder="Ingresa tu contraseña actual"
                                   Text="{Binding ContrasenaActual}"
                                   IsPassword="True"
                                   BackgroundColor="Transparent"/>
                        </Border>
                        <Label Text="La contraseña actual es incorrecta"
                               TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                            <Label.Triggers>
                                <DataTrigger TargetType="Label"
                                             Binding="{Binding IsContrasenaActualValida}" Value="False">
                                    <Setter Property="IsVisible" Value="True"/>
                                </DataTrigger>
                                <DataTrigger TargetType="Label"
                                             Binding="{Binding IsContrasenaActualValida}" Value="True">
                                    <Setter Property="IsVisible" Value="False"/>
                                </DataTrigger>
                            </Label.Triggers>
                        </Label>

                        <!-- Nueva contraseña -->
                        <Label Text="Nueva contraseña" FontSize="13"
                               TextColor="{StaticResource TextOnDarkSecondary}"/>
                        <Border BackgroundColor="{StaticResource GlassInputBackground}"
                                Stroke="{StaticResource GlassCardBorder}"
                                StrokeThickness="1" Padding="4,0">
                            <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                            <Border.Triggers>
                                <DataTrigger TargetType="Border"
                                             Binding="{Binding IsNuevaContrasenaValida}" Value="False">
                                    <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                                </DataTrigger>
                                <DataTrigger TargetType="Border"
                                             Binding="{Binding IsNuevaContrasenaValida}" Value="True">
                                    <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                                </DataTrigger>
                            </Border.Triggers>
                            <Entry Placeholder="Mínimo 6 caracteres"
                                   Text="{Binding NuevaContrasena}"
                                   IsPassword="True"
                                   BackgroundColor="Transparent"/>
                        </Border>

                        <!-- Confirmar nueva contraseña -->
                        <Label Text="Confirmar nueva contraseña" FontSize="13"
                               TextColor="{StaticResource TextOnDarkSecondary}"/>
                        <Border BackgroundColor="{StaticResource GlassInputBackground}"
                                Stroke="{StaticResource GlassCardBorder}"
                                StrokeThickness="1" Padding="4,0">
                            <Border.StrokeShape><RoundRectangle CornerRadius="10"/></Border.StrokeShape>
                            <Border.Triggers>
                                <DataTrigger TargetType="Border"
                                             Binding="{Binding IsNuevaContrasenaValida}" Value="False">
                                    <Setter Property="BackgroundColor" Value="{StaticResource GlassInputInvalid}"/>
                                </DataTrigger>
                                <DataTrigger TargetType="Border"
                                             Binding="{Binding IsNuevaContrasenaValida}" Value="True">
                                    <Setter Property="BackgroundColor" Value="{StaticResource GlassInputBackground}"/>
                                </DataTrigger>
                            </Border.Triggers>
                            <Entry Placeholder="Repite la nueva contraseña"
                                   Text="{Binding ConfirmarNuevaContrasena}"
                                   IsPassword="True"
                                   BackgroundColor="Transparent"/>
                        </Border>
                        <Label Text="Las contraseñas no coinciden o son muy cortas"
                               TextColor="{StaticResource ErrorRed}" FontSize="12" IsVisible="False">
                            <Label.Triggers>
                                <DataTrigger TargetType="Label"
                                             Binding="{Binding IsNuevaContrasenaValida}" Value="False">
                                    <Setter Property="IsVisible" Value="True"/>
                                </DataTrigger>
                                <DataTrigger TargetType="Label"
                                             Binding="{Binding IsNuevaContrasenaValida}" Value="True">
                                    <Setter Property="IsVisible" Value="False"/>
                                </DataTrigger>
                            </Label.Triggers>
                        </Label>

                    </VerticalStackLayout>
                </Frame>

                <!-- Botón cambiar contraseña -->
                <Button Text="Cambiar contraseña"
                        IsEnabled="{Binding IsFormCambioValido}"
                        Command="{Binding CambiarContrasenaCommand}">
                    <Button.Triggers>
                        <DataTrigger TargetType="Button"
                                     Binding="{Binding IsFormCambioValido}" Value="False">
                            <Setter Property="BackgroundColor" Value="#80607D8B"/>
                        </DataTrigger>
                    </Button.Triggers>
                </Button>

                <!-- Separador -->
                <BoxView HeightRequest="1" BackgroundColor="{StaticResource GlassCardBorder}"/>

                <!-- Cerrar sesión -->
                <Button Text="Cerrar sesión"
                        Style="{StaticResource DangerButton}"
                        Command="{Binding CerrarSesionCommand}"
                        Margin="0,4,0,0"/>

            </VerticalStackLayout>
        </ScrollView>

    </Grid>
</ContentPage>
```

- [ ] **Step 2: Build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s).

- [ ] **Step 3: Commit**

```bash
git add Views/Configuracion.xaml
git commit -m "style: glassmorphism Configuracion page"
```

---

## Final verification

- [ ] **Run full build**

```bash
dotnet build PettoV1/PettoV1.csproj
```

Expected: Build succeeded, 0 error(s), 0 warning(s) related to new changes.

- [ ] **Deploy to Android emulator or device and verify visually**

Check per page:
1. **Login / Registro** — gradient azul→cyan, formulario glass, botón activo con gradiente, botón deshabilitado gris translúcido, campos inválidos con fondo rojizo translúcido
2. **MainPage** — fondo teal oscuro→azul, card glass con avatar gradiente, círculos de acción translúcidos con sus colores originales
3. **Tareas** — labels uppercase cyan, lista en glass card, categorías como pills glass con línea inferior gradiente
4. **Chat** — burbujas de usuario con gradiente azul→cyan, burbujas IA glass, área de entrada glass
5. **Estadísticas** — 4 cards con tint de color (base glass, completadas cyan, pendientes naranja, progreso púrpura)
6. **Perfil** — avatar con gradiente circular + edit button cyan, info en glass card
7. **Configuración** — secciones con label uppercase cyan, campos glass, botón peligro rojo translúcido
8. **Flyout** — fondo oscuro, avatar glass, "Cerrar Sesión" con estilo peligro glass

- [ ] **Final commit**

```bash
git add -A
git commit -m "style: complete glassmorphism redesign for all Petto views"
```
