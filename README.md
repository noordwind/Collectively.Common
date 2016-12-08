# Coolector.Common

####**Keep your commune clean in just a few clicks.**

**What is Coolector?**
----------------

Have you ever felt unhappy or even angry about the litter left on the streets or in the woods? Or the damaged things that should've been fixed a long time ago, yet the city council might not even be aware of them?

**Coolector** is an open source & cross-platform solution that provides applications and a services made for all of the inhabitants to make them even more aware about keeping the community clean. 
Within a few clicks you can greatly improve the overall tidiness of the place where you live in. 

**Coolector** may help you not only to quickly submit a new remark about the pollution or broken stuff, but also to browse the already sent remarks and help to clean them up if you feel up to the task of keeping your neighborhood a clean place.

**Coolector.Common**
----------------

The **Coolector.Common** is a shared library referenced by the other Coolector services. It does provide some common interfaces, extensions, models and utilities.
It's being automatically published to the [MyGet](https://www.myget.org) after the new push to the repository.

**Solution structure**
----------------
- **Coolector.Common** - core project via that can be built via *dotnet build* command.
- **Coolector.Common.Tests** - unit & integration tests executable via *dotnet test* command.