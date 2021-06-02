<p align="center">
  <img src="https://github.com/M2-PWA-GAME/frontend/blob/master/home.png" alt="my-front screen"/>
</p>
<p align="center">
  <a href="https://david-dm.org/M2-PWA-GAME/frontend" alt="Dependencies"><img src="https://david-dm.org/M2-PWA-GAME/frontend.svg" /></a>
  <a href="http://www.gnu.org/licenses/gpl-3.0" alt="License: GPL v3"><img src="https://img.shields.io/badge/License-GPL%20v3-blue.svg" /></a>
</p>
<p align="center">
  <a href="https://github.com/M2-PWA-GAME/frontend/commits/master" alt="LastCommit"><img src="https://img.shields.io/github/last-commit/M2-PWA-GAME/frontend?style=flat-square" /></a>
  <a href="http://hits.dwyl.com/M2-PWA-GAME/frontend" alt="HitCount"><img src="http://hits.dwyl.com/M2-PWA-GAME/frontend.svg" /></a>
</p>

# Medieval Warfare - A game for the pleasure of the fight

Le but de ce projet est de mettre en place, à travers une Progressive Web App, un **petit jeu multijoueur** au tour par tour jouable avec des amis.

Le jeu sera complètement **asynchrone**, c'est-à-dire que chaque joueur jouera quand il lui plaira lorsque son tour survient pour des parties qui peuvent s'étendre sur des semaines entières.

## Built With

* [AspNetCore](https://github.com/dotnet/aspnetcore) - ASP.NET Core is an open-source and cross-platform framework for building modern cloud based applications.
* [Blazor](https://docs.microsoft.com/fr-fr/aspnet/core/blazor/?view=aspnetcore-5.0) - Blazor is an infrastructure for creating a client-side interactive Web user interface with .net


## Installation

```bash
# Check everything installed correctly
$ dotnet

## Output if succeed
Usage: dotnet [options]
Usage: dotnet [path-to-application]

Options:
-h|--help         Display help.
--info            Display .NET information.
--list-sdks       Display the installed SDKs.
--list-runtimes   Display the installed runtimes.

path-to-application:
The path to an application .dll file to execute.

## If not install dotnet
### First you’ll need to register the Microsoft key and feed:

$ wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
$ sudo dpkg -i packages-microsoft-prod.deb

### Then we’ll install the .NET Core SDK

$ sudo apt-get update; \
    sudo apt-get install -y apt-transport-https && \
    sudo apt-get update && \
    sudo apt-get install -y dotnet-sdk-5.0

### You can verify your install is correct by typing in:

dotnet --version

## If you have any probleme for dotNet installation please read documentation
[https://docs.microsoft.com/en-us/dotnet/core/install/linux]
[https://dotnet.microsoft.com/download]
[https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/install]
```

## Development setup


```bash
# serve with hot reload at localhost:5000
$ dotnet watch run
```

## Organisation
```bash
.
├─ .github             -> 
├─ Components          -> 
├─ Model               -> 
├─ Pages               -> 
├─ Properties          -> 
├─ Shared              -> 
└─ wwwroot             -> 
```

## Release History

* 0.1
    * MVP

## Authors

* Alban PIERSON - [@Zalbani](https://github.com/Zalbani)
* Brice MICHALSKI - [@BriceMichalski](https://github.com/BriceMichalski)  
* Benjamin L'HONNEN - [@BenjaminLHONNEN](https://github.com/BenjaminLHONNEN)
* Martin PELCAT - [@MartinPELCAT](https://github.com/MartinPELCAT)


## License

This project is licensed under the GNU GPL v3 License - see the [LICENSE.md](LICENSE.md) file for details