# Database-Interaction-with-ADO.NET

An application to interact with the database developed in https://github.com/monicahomescu/Ballet-Studio-Database-in-Sql. It contains a form allowing the user to prepare two different scenarios handling data from two different `1:n relationships`. The form is generic enough such that switching between scenarios (i.e., updating the application to handle data from another 1:n relationship) can be achieved by simply updating the `configuration file`. A Panel is used for dynamically generating the Textboxes, given that not all the child tables have the same number of columns.  

The following functionalities are provided:

- display all the records in the parent table;
- display the child records for a specific (i.e., selected) parent record;
- add / remove / update child records for a specific parent record.

<img width="50%" height="50%" src="/demos/demo1.PNG" width="80%">

<img align="left" width="100%" height="100%" src="/demos/demo2.PNG" width="80%">

<img width="100%" height="100%" src="/demos/demo3.PNG" width="80%">
