using System;

class Program {
    public static void Main (string[] args) {
        Menger mydata = new Menger("mydata");
        mydata.addItem("root", "new item");
        mydata.addItem("root/new item", "another item");


        
        mydata.printAll("root");
        Console.WriteLine(mydata.data);
    }
}