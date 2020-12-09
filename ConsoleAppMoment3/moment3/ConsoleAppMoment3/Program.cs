using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Xml.Serialization;
using static System.Console;

namespace ConsoleAppMoment3 //Creating namesopace/ adress of type 
{
    class Program //Creating class Program 
    {
        static void Main(string[] args) /* method Main, Enterypioint for application, with array string[]args to pass arguments into the console application */ 
        {
            App app = new App("guestbook.dat"); /*Instantiating an object of type app with file name of guestbook as argument to the constructor*/ 

            app.RunMenu(); //call the method RunMenu in the object app

        }
    }

    class Person //Creating class Person 
    {
        private string name; //Declairing a field of type string with name "name" and visability private

        public Person(string name) // Creating constructor for class Person with a string argument 
        {
            this.name = name;// the class field name is asigned to the value of the argument name
        }
        public string GetName() //Getter for field name with return value of type string
        {
            return this.name;
        }
    }
    [Serializable] // Tells the compiler that it is possible to serialize this class
    public class Post
    {
        public string content; //Declairing a field of type string with name "content" & "name" and visability private
        public string name;

        public void setContent(string content) //Method that takes an argument of type string with name "content"
        {
            this.content = content; // the class field content is asigned to the value of the argument content
        }
        public void setName(string name)
        {
            this.name = name;
        }
        public string GetContent()
        {
            return this.content; //Method GetContent returns the value of field content
        }
        public string GetPerson()
        {
            return this.name;
        }


    }
    class App
    {
        private List<Post> posts = new List<Post>(); /*Field of type "list" that contains type <Post> with name posts
                                                       * New instance of class list with class list containing Post*/
        private string filename;  // Declairing a field of type string with name "filename" and visability private

        public App(string filename) // Creating constructor for class App with a string argument 
        {
            this.filename = filename; // the class field filename is asigned to the value of the argument filename
            if (File.Exists(this.filename)) // check if file exist with argument of the value of class field filename
            {
                FileStream stream = File.OpenRead(this.filename); /* call the method OpenRead in the class File with this.filename as
                                                                   * argument this method OpenRead returns an instance of 
                                                                   * class FileSttream and stores this instance in variable stream */
                var formatter = new BinaryFormatter();
                var v = (List<Post>)formatter.Deserialize(stream);
                this.posts = v;
                stream.Close(); // Closes the file stream to avoid stack overflow
            }
        }

        public void RunMenu()
        {
            bool keepRunning = true; // flag that is used to keep the main loop running as long as keepRunning evalutes to true
            while (keepRunning) 
            {
                PrintMenu();
                PrintPosts();
                char menuVal;

                while (!Char.TryParse(Console.ReadLine(), out menuVal)) ;
                //validera indata för meny val 1 2 eller x
                switch (menuVal)
                {
                    case '1':
                        DoPost();
                        break;
                    case '2':
                        RemovePost();
                        break;
                    case 'x':
                        keepRunning = false;
                        SavePosts();
                        break;
                    default:
                        Console.WriteLine("Ogiltig input");
                        break;

                }
                Console.Clear();
            }
        }

        private bool RemovePost()
        {
            int menuVal;
            do
            {
                Console.WriteLine("Vänligen ange nummert på den posten som du vill radera");
            }
            while (!int.TryParse(Console.ReadLine(), out menuVal));/*do while loop is running as
                                                                    *long as an integer is not given from the console */
            if (menuVal < this.posts.Count)/*Check if integer given is less than number of existing posts*/
            {
                this.posts.RemoveAt(menuVal); /*if it is the post is removed*/ 
            }
            return true;

        }

        private void PrintMenu()
        {
            Console.WriteLine("Cristinas gästbook");
            Console.WriteLine("1. Skriv i gästbok");
            Console.WriteLine("2. Ta bort inlägg");
            Console.WriteLine("x. Avsluta");
        }
        private void PrintPosts()
        {

            int i = 0;
            foreach (Post post in this.posts)// Iterate through each post in the field posts that is a list of posts
            {
                Console.WriteLine("[" + i++ + "] " + post.GetPerson() + " - " + post.GetContent());
            }
            
                
        }

        private bool DoPost()
        {

            string name = "";
            do
            {
                Console.WriteLine("Vänligen ange ett namn");
                name = Console.ReadLine();
            } while (String.IsNullOrEmpty(name)); //Method of class string that checks if argument is null or mepty 
            Person person = new Person(name);
            string content = "";
            do
            {
                Console.WriteLine("Vänligen ange innehåll");
                content = Console.ReadLine();
            } while (String.IsNullOrEmpty(name));
            Post post = new Post();
            post.setContent(content); //Call method setContent on variable post that is of type post 
            post.setName(person.GetName());
            posts.Add(post);
            return true;
        }
        private void SavePosts()
        {
            FileStream stream = File.OpenWrite(this.filename);/*Open new file towrite to
                                                               * Shoos formatter to use for serialization
                                                               *use formatter method serialize to serialize the feild this.posts
                                                               * that is a list of Post object */
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, this.posts);
            stream.Close();
            Console.Clear(); //Rewrite consol 
        }
    }
}
