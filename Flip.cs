// Author: Jeremiah Kalmus
// File: Flip.cs
// Date: June 7th, 2019
// Version: 2.0

/* 
 * OVERVIEW:
 *  Flip during construction takes 1 or zero parameter values and defines an encapsulated_string and a
 *  The client can then enter a non-negative integer and Flip will flip the encapsulated_strings up to the entered digits
 *  character.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * DESIGN DECISIONS AND ASSUMPTIONS:
 *  1 - flip_value of 0 will simply do nothing.
 *  
 *  2 - flip_value greater than the length of the encapsulated_string will simply slip the entire string.
 *      
 *  3 - flip_value cannot be negative
 *      
 * ---------------------------------------------------------------------------------------------------------------------------------
 * INTERFACE INVARIANTS:
 *  1 - Flip_Password:
 *
 * 		Enter a value into the Flip_Password method and the encapsulated_string characters will flip from the beginning of
 * 		the string to the value entered into the mathod. Values entered must be non-negative integers. 
 * ---------------------------------------------------------------------------------------------------------------------------------
 * IMPLEMENTATION INVARIANTS:
 *  1 - encapsulated_string:
 *
 * 		This data member is never changed after it is set (using readonly) and flipped_string is manipulated to achieve the desired effect.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * CLASS INVARIANTS:
 *  1 - CONSTRUCTORS:
 *  
 *      The constructor takes in 1 parameter which is the encapsulated_string that will be flipped in Flip_Password when called.
 *      
 *  2 - Flip_Password:
 *
 *		Is an unsigned integer that can be any value and will take all characters in the encapsulated_string up to the flip_digit 
 * 		number character in the string and flip that part of the string. If 0 or 1 is entered, then the string will be identical
 * 		to the dafult encapsulated_string (nothing will flip for 0 and the first character will flip with itself which is the same
 * 		result). If the flip_digit is equal or larger than the length of the encapsulated_string, then the whole string will be flipped_string
 * 		regardless of how much larger flip_digit is than the length of the encapsulated_string.  
 * ---------------------------------------------------------------------------------------------------------------------------------
 */
/* 
* UPDATES:
* 
* 6/5/2019 - Replaced string flipped_string with char[] flipped_string and added char[] encapsulated_temp.
* 
*/

using System;

namespace p6
{
    class Flip
    {
        private const string DEFAULT_STR = "Default";
        private readonly string encapsulated_string;
        private readonly char[] encapsulated_temp;
        private char[] flipped_string;

        public Flip(string input_encapsulated_string = DEFAULT_STR)
        {
            encapsulated_string = input_encapsulated_string;
            encapsulated_temp = encapsulated_string.ToCharArray();
            flipped_string = encapsulated_string.ToCharArray();
        }
        // POST: Flipped string will change from being equal to the string entered during construction
        //		 to the appropriate flipped string.
        public string Flip_Password(uint flip_digit)
        {
            string return_string = "";
            if (flip_digit >= encapsulated_string.Length)
            {
                for (uint i = 0; i < encapsulated_string.Length; i++)
                {
                    flipped_string[i] = encapsulated_temp[(encapsulated_string.Length - 1) - i];
                }
            }
            else
            {
                for (uint i = 0; i < flip_digit; i++)
                {
                    flipped_string[i] = encapsulated_temp[(flip_digit - 1) - i];
                }
            }
            for (uint i = 0; i < encapsulated_string.Length; i++)
            {
                return_string += flipped_string[i];
            }
            return return_string;
        }
    }
}