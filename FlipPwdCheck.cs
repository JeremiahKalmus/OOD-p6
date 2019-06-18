// Author: Jeremiah Kalmus
// File: FlipPwdCheck.cs
// Date: June 7th, 2019
// Version: 2.0

/* 
 * OVERVIEW:
 *  FlipPwdCheck will take the encapsulated password, flip the first 'flip_digit' amount of characters in the string, and then
 * 	compare that result with IPwdCheck interface to see if it passes or not.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * DESIGN DECISIONS AND ASSUMPTIONS:
 *  1 - Check_Flipped_Password:
 * 		
 *      This method will flip first and then compare the flipped password and return a boolean to the client whether it passed the
 * 		check or not. 
 * 		
 *  2 - IPwdCheck
 *  
 *      Made an interface IPwdCheck so FlipPwdCheck could use that public functionality while inheriting from Flip.
 *      Also, FlipPwdCheck can take a polymorphic handle of type pwdCheck in order to have pwdCheck, excessC, and compundC
 *      objects be able to be passed into this class.
 *      
 * ---------------------------------------------------------------------------------------------------------------------------------
 * INTERFACE INVARIANTS:
 *  1 - Check_Flipped_Password:
 *
 * 		Enter a value into the Flip_Password method and the encapsulated_string characters will flip from the beginning of
 * 		the string to the value entered into the mathod. Afterwards, the flipped string will be checked to see if it meets certain
 * 		specifications (reference pwdCheck for specifications). Values entered can be any non-negative integer.  
 * 
 * 	2 - Reference pwdChek and Flip for additional functionality.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * IMPLEMENTATION INVARIANTS:
 * 	1 - Reference pwdChek and Flip for additional functionality.
 * 	
 * ---------------------------------------------------------------------------------------------------------------------------------
 * CLASS INVARIANTS:
 *  1 - CONSTRUCTORS:
 *  
 *      The constructor takes in 2 parameters which are the encapsulated_string that will be flipped in Flip_Password when called and
 * 		a pwdCheck type object (pwdCheck or it's descendents).
 *
 *  2 - Check_Flipped_Password:
 *
 *		Is an unsigned integer that can be any value and will take all characters in the encapsulated_string up to the flip_digit 
 * 		number character in the string and flip that part of the string. If 0 or 1 is entered, then the string will be identical
 * 		to the dafult encapsulated_string (nothing will flip for 0 and the first character will flip with itself which is the same
 * 		result). If the flip_digit is equal or larger than the length of the encapsulated_string, then the whole string will be flipped_string
 * 		regardless of how much larger flip_digit is than the length of the encapsulated_string (see Flip for further information). Afterwards, the 
 * 		method will then take the returned flipped string and check that against the pwdCheck specifications (see pwdCheck for further information).
 * ---------------------------------------------------------------------------------------------------------------------------------
 */
/* 
* UPDATES:
* 
* 6/5/2019 - Inherits from pwdCheck and IFLip interface. A public Flip_Password method was created to echo the Flip
*            class Flip_Password functionality. 
* 
*/
using System;

namespace p6
{
    class FlipPwdCheck : Flip, IpwdCheck
    {
        pwdCheck pwd_obj;
        public FlipPwdCheck(string input_encapsulated_string, pwdCheck input_pwd_obj) : base(input_encapsulated_string)
        {
            pwd_obj = new pwdCheck();
            pwd_obj = input_pwd_obj;
        }
        // PRE: Password needs to be entered by the client							 
        // POST: Object state may be toggled and state_counter may be incremented							 
        public bool Check_Flipped_Password(uint flip_digit)
        {
            return Password_Check(Flip_Password(flip_digit));
        }
        // PRE: Password needs to be entered by the client
        // POST: Object state may be toggled and state_counter may be incremented
        public bool Password_Check(string input_password)
        {
            return pwd_obj.Password_Check(input_password);
        }
        public uint Password_Length
        {
            get
            {
                return pwd_obj.Password_Length;
            }
        }
        public uint Minimum_Password_Length
        {
            get
            {
                return pwd_obj.Minimum_Password_Length;
            }
        }
        public bool isActive
        {
            get
            {
                return pwd_obj.isActive;
            }
        }
    }
}