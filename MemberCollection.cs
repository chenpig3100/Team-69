using System;


namespace Team_69
{
    public class MemberCollection
    {
        //setting fields
        //declaring the compacity of the memberCollection array 
        private const int MaxMembers = 500;

        //the actual array
        private Member[] members = new Member[MaxMembers];

        //for counting existing members
        private int count = 0;

        //the property variable of the count, used for when a member is searching for total number of members
        public int Count
        {
            get { return count; }
        }

        //methods
        //Adding a new member, accepting the property variables of a member
        public bool Add(string firstName, string lastName, string password, string phoneNumber)
        {
            //check if the array is already full without assuming it's always the last element, return an error message
            if (count >= MaxMembers)
            {
                Console.WriteLine("We have reached the maximum compacity of members, please contact the staff.");
                return false;
            }

            //check if the registering user has already existed in the member array
            for (int i = 0; i<count; i++)//loop thru the existing users first
            {
                //if the passed in arguments matched the array elements, prompt error message
                if (members[i].FirstName == firstName && members[i].LastName == lastName)
                {
                    Console.WriteLine("Member already exists.");
                    return false;
                }

            }


            //adding a new member into the array
            members[count] = new Member(firstName, lastName, password, phoneNumber);
            count++;
            Console.WriteLine($"New member {firstName} {lastName} registered successfully.");
            return true;

        }

        public bool Find(string firstName, string lastName)
        {
            //if there's no matching member by the first name and last name, then return an error message
            for (int i = 0; i < count; i++)//loop thru the array
            {
                //if found the member then return their name and phone number
                if (members[i].FirstName == firstName && members[i].LastName == lastName)
                {
                    Console.WriteLine($"Member found: {firstName} {lastName}");
                    Console.WriteLine($"Contact number: {members[i].PhoneNumber}");
                    return true;
                }

            }

            //if didn't find the member then output error message
            Console.WriteLine("Member not found.");
            return false;


        }

        public bool Remove(string firstName, string lastName)
        {
            //if there's no matching member by the first name and last name, then return an error message
            for (int i = 0; i < count; i++)//loop thru the array
            {
                //if found the member then remove it from the member array, and assign the last element of the array to the deleted space
                if (members[i].FirstName == firstName && members[i].LastName == lastName)
                {
                    //member must return all borrowed DVDs before being remove
                    if (members[i].BorrowedCount > 0)
                    {
                        Console.WriteLine("This member must return all borrowed DVDs before being remove");
                        return false;
                    }
                    
                    //actually removing the member, by replacing the to-be-removed element with the last element
                    members[i] = members[count - 1];
                    members[count - 1] = null;
                    count--;
                    return true;
                }

            }

            //if didn't find the member then output error message
            Console.WriteLine("Member not found.");
            return false;
        }

        //Finding members, wrote in here "MemberCollection"
        public Member? FindByNameAndPassword(string fname, string lname, string password)
        {
            for (int i=0; i< count; i++)
            {
                if (members[i] != null &&
                    members[i].FirstName == fname &&
                    members[i].LastName == lname &&
                    members[i].Password == password)
                {
                    return members[i];
                }
            }

            return null;
           
        }

    }

}
