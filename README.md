# Question 5
Write a C# Web API that:
- returns a list of countries with from the IList class using the get method
- adds a country to the list using the POST method
- returns a particular country using its index from the list i.e get/{id}

## Brief Explanation
I was able to create an XML file (database.xml) to act as my data source and placed it in my root folder. Then I read from the XML file and casted it to an IList class.

The default route for the API calls is:
http://localhost:62952/api/country

## API Screenshots
Get Method - Returns a list of countries <br />
![alt text](https://gtbtech5.s3.us-east-2.amazonaws.com/Q5A.PNG)<br />

Get Method - Returns a particular country using its index from the list <br />
![alt text](https://gtbtech5.s3.us-east-2.amazonaws.com/Q5B.PNG) <br />

Post Method - Adds a country to the list using the POST method <br />
![alt text](https://gtbtech5.s3.us-east-2.amazonaws.com/Q5C.PNG)  <br />
