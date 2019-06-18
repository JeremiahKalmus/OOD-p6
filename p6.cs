// Author: Jeremiah Kalmus
// File: p6.cs
// Date: June 7th, 2019
// Version: 1.0

/*
 * OVERVIEW:
 * 
 * This driver tests the simulated multiple inheritance using interfaces with FlipPwdCheck. A file with passwords will be read
 * into an array and a heterogeneous collection of PwdCheck, compundC, and excessC objects will be created to pass into FlipPwdCheck
 * objects. FlipPwdCheck objects take in a PwdCheck type object which is a polymorphic handle to any of the PwdCheck, compundC, or
 * excessC types. A heterogeneous collection of type Flip taking in Flip and FlipPwdCheck objects will be created for testing. Next,
 * a method will check to see if an object in the Flip array is of type FlipPwdCheck or not. Depending on the answer, the object will
 * be passed into a method that will test the respective object public functionality. For Flip objects, testing the public Flip_Password
 * functionality will be tested. For FlipPwdCheck objects, Flip_Password, Check_Flipped_Password, and the public accessors will be
 * tested. Also, for FlipPwdCheck objects, the state of the object will be forced to change and then the functionality of the new state
 * will be tested as well.
 * 
 */

using System;
using System.IO;

namespace p6
{
    class p6
    {
        const string ASTERISK = "***************************************";
        const uint TEST_OBJ_ARRAY_SIZE = 6;
        const uint SIZE_OF_PASSWORD_FILE = 10;
        const int RANDOM_PASSWORD_LENGTH = 30;
        const int RANDOM_FLIP_DIGIT_MAX = 12;
        const string POSSIBLE_ASCII = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz0123456789!\"%&'()*+,-./:;<=>?@[]\\^_`|{}~ $#";
        const string FILE_PATH = "test.txt";

        static string[] Import_Password_File()
        {
            Console.WriteLine("Reading in a file of passwords...");
            Console.WriteLine();

            string[] password_array = File.ReadAllLines(FILE_PATH);

            Console.WriteLine(ASTERISK);
            Console.WriteLine("Press enter to continue program...");
            Console.WriteLine(ASTERISK);
            Console.ReadLine();

            return password_array;
        }
        static pwdCheck[] Initialize_Pwdcheck_Object_Array(Random rand)
        {
            Console.WriteLine("Creating a heterogeneous collection of pwdCheck, compundC, and excessC objects to");
            Console.WriteLine("input into FlipPwdCheck objects...");
            Console.WriteLine();
            pwdCheck[] initializer_array = new pwdCheck[TEST_OBJ_ARRAY_SIZE];

            const int MAX_STATE_CHANGES = 10;
            uint random_password_length;
            int random_valid_ascii_index;
            uint max_state_changes;

            for (uint i = 0; i < TEST_OBJ_ARRAY_SIZE; i++)
            {
                random_password_length = (uint)rand.Next(0, RANDOM_PASSWORD_LENGTH);
                random_valid_ascii_index = rand.Next(0, POSSIBLE_ASCII.Length);
                max_state_changes = (uint)rand.Next(0, MAX_STATE_CHANGES);
                if (i < TEST_OBJ_ARRAY_SIZE / 3)
                {
                    initializer_array[i] = new pwdCheck(random_password_length, POSSIBLE_ASCII[random_valid_ascii_index]);
                    Console.WriteLine("Object " + i + " is a pwdCheck object.");
                }
                else if ((TEST_OBJ_ARRAY_SIZE / 3) <= i && TEST_OBJ_ARRAY_SIZE - (TEST_OBJ_ARRAY_SIZE / 3) > i)
                {
                    initializer_array[i] = new excessC(random_password_length, POSSIBLE_ASCII[random_valid_ascii_index]);
                    Console.WriteLine("Object " + i + " is an excessC object.");
                }
                else
                {
                    initializer_array[i] = new compundC(max_state_changes, random_password_length,
                                                       POSSIBLE_ASCII[random_valid_ascii_index]);
                    Console.WriteLine("Object " + i + " is a compundC object.");
                }
                Console.WriteLine("Object " + i + " took in a password of length: " + random_password_length);
                Console.WriteLine("After construction, object " + i + " has a password length requirement of: " +
                                    initializer_array[i].Minimum_Password_Length);
                Console.WriteLine("Object " + i + " has a forbidden character: " + POSSIBLE_ASCII[random_valid_ascii_index]);
                Console.WriteLine();
            }
            Console.WriteLine(ASTERISK);
            Console.WriteLine("Press enter to continue program...");
            Console.WriteLine(ASTERISK);
            Console.ReadLine();
            return initializer_array;
        }
        static Flip[] Initialize_Flip_Object_Array(Random rand, pwdCheck[] pwd_check_object_array, string[] password_array)
        {
            Console.WriteLine("Creating heterogeneous collection of Flip and FlipPwdCheck objects for testing...");
            Console.WriteLine();

            Flip[] initializer_array = new Flip[TEST_OBJ_ARRAY_SIZE];
            int pwd_index;
            int pwd_obj_array_index;

            for (uint i = 0; i < TEST_OBJ_ARRAY_SIZE; i++)
            {
                pwd_index = rand.Next(0, (int)SIZE_OF_PASSWORD_FILE);

                if (i < TEST_OBJ_ARRAY_SIZE/2)
                {
                    Console.WriteLine("Object " + i + " encapsulated password is: " + password_array[pwd_index]);
                    initializer_array[i] = new Flip(password_array[pwd_index]);
                }
                else
                {
                    Console.WriteLine("Object " + i + " encapsulated password is: " + password_array[pwd_index]);
                    pwd_obj_array_index = rand.Next(0, (int)TEST_OBJ_ARRAY_SIZE);
                    initializer_array[i] = new FlipPwdCheck(password_array[pwd_index], pwd_check_object_array[pwd_obj_array_index]);
                    if (pwd_check_object_array[pwd_obj_array_index] is compundC)
                    {
                        Console.WriteLine("Object type entered into the Flip object is of type: copmundC");
                    }
                    else if (pwd_check_object_array[pwd_obj_array_index] is excessC)
                    {
                        Console.WriteLine("Object type entered into the Flip object is of type: excessC");
                    }
                    else
                    {
                        Console.WriteLine("Object type entered into the Flip object is of type: pwdCheck");
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine(ASTERISK);
            Console.WriteLine("Press enter to continue program...");
            Console.WriteLine(ASTERISK);
            Console.ReadLine();
            return initializer_array;
        }
        static void Test_Flip_Pwd_Check_Obj_Array(Random rand, Flip[] flip_object_array)
        {
            Console.WriteLine("Testing heterogeneous collection of Flip and FlipPwdCheck objects...");
            Console.WriteLine();

            for (int i = 0; i < TEST_OBJ_ARRAY_SIZE; i++)
            {
                if (flip_object_array[i] is FlipPwdCheck)
                {
                    Console.WriteLine("Object " + i + " is a FlipPwdCheck object");
                    Test_Flip_Pwd_Check_Obj(rand, (FlipPwdCheck)flip_object_array[i]);
                }
                else
                {
                    Console.WriteLine("Object " + i + " is a Flip object");
                    Test_Flip_Obj(rand, flip_object_array[i]);
                }
            }
            Console.WriteLine(ASTERISK);
            Console.WriteLine("Press enter to end program...");
            Console.ReadLine();
        }
        static void Test_Flip_Obj(Random rand, Flip flip_object)
        {
            uint flip_digit = (uint)rand.Next(0, RANDOM_FLIP_DIGIT_MAX);
            Console.WriteLine("Encapsulated password flipped before character #: " + flip_digit);
            Console.WriteLine("Flipped password: " + flip_object.Flip_Password(flip_digit));
            Console.WriteLine();
        }
        static void Test_Flip_Pwd_Check_Obj(Random rand, FlipPwdCheck flipPwdCheck_object)
        {
            uint flip_digit = (uint)rand.Next(0, RANDOM_FLIP_DIGIT_MAX);
            Console.WriteLine("Encapsulated password flipped before character #: " + flip_digit);
            Console.WriteLine("Flipped password: " + flipPwdCheck_object.Flip_Password(flip_digit));
            Console.WriteLine("Checking if the flipped password passes the pwdCheck: " + 
                flipPwdCheck_object.Check_Flipped_Password(flip_digit));
            Console.WriteLine("P value is: " + flipPwdCheck_object.Minimum_Password_Length);
            Console.WriteLine("Length of encapsulated pwd is: " + flipPwdCheck_object.Password_Length);
            Console.WriteLine("Is this object active?: " + flipPwdCheck_object.isActive);
            Console.WriteLine();
            Console.WriteLine("Now I will force this object to change state and then test its chaged functionality");
            Console.WriteLine();
            Console.WriteLine(ASTERISK);
            Console.WriteLine("Checking flipped password " + flipPwdCheck_object.Minimum_Password_Length + " times...");
            Console.WriteLine();


            for (uint i = 0; i < flipPwdCheck_object.Minimum_Password_Length; i++)
            {
                flip_digit = (uint)rand.Next(0, RANDOM_FLIP_DIGIT_MAX);
                Console.WriteLine("Encapsulated password flipped before character #: " + flip_digit);
                Console.WriteLine("Flipped password: " + flipPwdCheck_object.Flip_Password(flip_digit));
                Console.WriteLine("Checking if the flipped password passes the pwdCheck: " + 
                    flipPwdCheck_object.Check_Flipped_Password(flip_digit));
                Console.WriteLine("Is this object active?: " + flipPwdCheck_object.isActive);
                Console.WriteLine();
            }
            Console.WriteLine(ASTERISK);
            Console.WriteLine("The object has changed state. Testing its new functionality...");
            Console.WriteLine(ASTERISK);
            Console.WriteLine();


            flip_digit = (uint)rand.Next(0, RANDOM_FLIP_DIGIT_MAX);
            Console.WriteLine("Encapsulated password flipped before character #: " + flip_digit);
            Console.WriteLine("Flipped password: " + flipPwdCheck_object.Flip_Password(flip_digit));
            Console.WriteLine("Checking if the flipped password passes the pwdCheck: " +
                flipPwdCheck_object.Check_Flipped_Password(flip_digit));
            Console.WriteLine("P value is: " + flipPwdCheck_object.Minimum_Password_Length);
            Console.WriteLine("Length of encapsulated pwd is: " + flipPwdCheck_object.Password_Length);
            Console.WriteLine("Is this object active?: " + flipPwdCheck_object.isActive);
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            Random rand = new Random();
            string[] password_array = Import_Password_File();
            pwdCheck[] pwd_check_object_array = Initialize_Pwdcheck_Object_Array(rand);
            Flip[] flip_object_array = Initialize_Flip_Object_Array(rand, pwd_check_object_array, password_array);
            Test_Flip_Pwd_Check_Obj_Array(rand, flip_object_array);
        }
    }
}