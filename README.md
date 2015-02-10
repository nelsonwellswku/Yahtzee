Yahtzee in C#
===================

[Yahtzee](http://en.wikipedia.org/wiki/Yahtzee) is a dice game by Milton Bradley
where the goal is to make certain combinations of dice to get the highest point total.
This is a single-player browser based implemention built with C#, ASP.NET MVC 5, and SignalR.

Demo
----

The current _dev_ branch will always be hosted on AppHarbor.

*[Yahtzee Demo](http://yahtzee.apphb.com)*

Building
--------
The solution can be built with [Visual Studio 2013 Community Edition](http://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx) out of the box.
LocalDB 2014 is required for the Entity Framework powered auth bits. Everything else should be downloaded from Nuget automatically as long as you're using
the latest VS service pack and Nuget 2.7 or better.

The unit tests can be run with the [NUnit test runner](http://www.nunit.org/index.php?p=nunit-gui&r=2.4).
 
License
-------
All original code is licensed under the MIT license. See license.txt for the full text.