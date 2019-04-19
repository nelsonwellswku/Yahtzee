Yahtzee in C#
===================

[Yahtzee](http://en.wikipedia.org/wiki/Yahtzee) is a dice game by Milton Bradley
where the goal is to make certain combinations of dice to get the highest point total.
This is a single-player browser based implemention built with C#, ASP.NET MVC 5, and SignalR.

Demo
----

The current _dev_ branch is hosted an Azure App Service.

*[Yahtzee Demo](https://yahtzee.azurewebsites.net/)*

Building
--------
The solution can be built with [Visual Studio 2019 Community Edition](http://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx).


The unit tests can be run inside Visual Studio with the [XUnit runner](https://www.nuget.org/packages/xunit.runner.visualstudio/).

Entity Framework 6 code-first migrations can be run against a recent version of SQL Server.

License
-------
All original code is licensed under the MIT license. See license.txt for the full text.