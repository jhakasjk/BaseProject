# BaseProject
Base Project Architecture with ASP.Net MVC5, Web-API 2 &amp; Ninject IoC container

Prerequisites:-
1. Visual Studio 2013 or higher.
2. .Net Framework 4.5.2
3. Entity Framework 6

Description:- 
Solution is having 5 projects under it.

Base Project:- 
It's a ASP.NET MVC 5 WebAPI 2 project having Controllers & Views. I have implemented DI with Ninject IoC container with naming conventions in this project.

BaseProject.WebAPI:- 
It's a separate project for making RESTfull services using WebApi 2.

MvcGlobalisationSupport:- 
This is a class library project to provide multi lingual support to main web project.

CoreEntities:- 
It's a class library project having entities defined in it. It also has Models required for MVC project.

BusinessEntities:- 
It's a class Library Project having the imnplementation of each CoreEntities's Interface methods & some utility methods.

DataAccessLayer:- 
It's a class library project having database as entity using entity framework 6.0.

EmailSubscriptionScheduler:- 
It's console application for making API calls to WebApi 2 project (BaseProject)
