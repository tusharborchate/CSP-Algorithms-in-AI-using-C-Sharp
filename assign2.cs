/// *    Note


// Compiler used -  C# compiler (csc)

//steps to execute program :

// 1 . This program contains lot of for loops and extensive search which needs lot of memory.
       //online compiler might not work.please  use visual studio to run.

// 2.  Copy and paste all source code from assign2.cs

// 3. developer: tushar borchate/ 200393116 /306-529-7874
// This code contains all comments describing logic.

//4. please clear all code then copy and paste

//5. Try to use simple inputs :P

//6.Last update : 04/03/18 23 34
//////////////////////////////////////////////////////////////////////////

//  code starts here ////////


using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    //main class
     public  class CSP
    {
        static void Main(string[] args)
        {

            //assignment no 2 code starts here
            Console.WriteLine("Assignment No:2 Implementing CSP search strategies (new feature -  use same setting,csp and compare)");

            Console.WriteLine("--------------------------------------------------------");

            //creating object of logic class
            CSPLogic cspLogic = new CSPLogic();

            //give input and select method
            cspLogic.ChooseMethod();
            Console.ReadLine();
        }

        //logic class
        public class CSPLogic
        {
            //solution found
            bool solutionFound = true;

            //track number of variables
            int noVariables = 0;
            //track number of variables
            float constraintTightness = 0;
            //track consatntA
            float constantA = 0;
            //track constantR
            float constantR = 0;
            //track number of domains
            int domainSize = 0;
            //track number of constraints
            int noOfConstraint = 0;
            //track number of tuples
            int noOftuples = 0;
            //track variable list to display
            List<string> variablesList = new List<string>();
            //track domain list to display
            List<int> domainList = new List<int>();
            //track constraintsmodellist
            List<CSPConstraintsModel> cspConstraintModelList = new List<CSPConstraintsModel>();
            //to track if has solution
            int nosolution = 0;
            //to track variable with position and domain
            List<CSPVariableModel> CSPVariableModelList = new List<CSPVariableModel>();

            //track time taken
            DateTime d;
            DateTime d1;
            int oldVariable = 0;
            //track time taken
            int milliSeconds = 0;
            //track nodes searched
            int iterationTaken = 0;
            //no of backtracks
            int nobackTracks = 0;
            //arc consistancy 
            bool checkarcConsistancy = false;
            // no of revisedconsistancyperformed
            int reviseConsistancy = 0;


            #region input
            //select method
            public void ChooseMethod()
            {
                try
                {
                    //to track if exit
                    bool exit = false;

                  
                    while (!exit)
                    {
                        //getting input first

                        //to track result of given input
                        bool result = false;

                        //track if same setting
                        string sameSetting = "0";
                        if (iterationTaken != 0)
                        {
                            Console.WriteLine("Do you want same variable settings or new?(1-yes/2-no/other key-exit)");
                            sameSetting = (Console.ReadLine());

                            if (sameSetting == "1")
                            {
                                iterationTaken = 0;
                                milliSeconds = 0;
                                nobackTracks = 0;
                                solutionFound = true;
                                reviseConsistancy = 0;
                                foreach (var item in CSPVariableModelList)
                                {
                                    item.VariablePosition = 0;
                                    item.ArcDomainList = new List<int>();
                                    item.ArcDomainList = domainList;

                                }
                                nosolution = 0;
                            }
                            else if (sameSetting == "2")
                            {

                                reviseConsistancy = 0;
                                nobackTracks = 0;
                                iterationTaken = 0;
                                //track number of variables
                                noVariables = 0;
                                //track number of variables
                                constraintTightness = 0;
                                //track consatntA
                                constantA = 0;
                                //track constantR
                                constantR = 0;
                                //track number of domains
                                domainSize = 0;
                                //track number of constraints
                                noOfConstraint = 0;
                                //track number of tuples
                                noOftuples = 0;
                                //track variable list to display
                                variablesList = new List<string>();
                                //track domain list to display
                                domainList = new List<int>();
                                //track constraintsmodellist
                                cspConstraintModelList = new List<CSPConstraintsModel>();
                                //track cspvariablemodellist
                                CSPVariableModelList = new List<CSPVariableModel>();
                                oldVariable = 0;
                                milliSeconds = 0;
                                iterationTaken = 0;
                                nosolution = 0;
                                solutionFound = true;
                            }
                            else
                            {
                                Environment.Exit(1);
                            }
                        }
                        //if  csp not solvable then ask input again
                        while (!result && iterationTaken == 0 && sameSetting != "1")
                        {
                           //get input
                            GetCSPInput();
                            //check phase transition
                            result = CheckPhaseTransition();
                            //if p<pt then generate random csp
                            if (result)
                            {
                                GenerateRandomCSP();
                                DisplayCSP();
                            }
                            else
                            {
                                variablesList = new List<string>();
                                domainList = new List<int>();
                                CSPVariableModelList = new List<CSPVariableModel>();

                            }
                        }

                        Console.WriteLine("----------------------------------------------------------------");
                        result = false;
                        bool AC3Result = true;
                        while (!result)
                        {
                            Console.WriteLine("Do you want Arc Consistancy (AC) algorithm to run before CSP search?(1-yes/2-no)");
                            string arcConsistancy = (Console.ReadLine());
                            if (arcConsistancy == "1")
                            {
                                checkarcConsistancy = true;
                              AC3Result=  AC3Algorithm();
                                if (AC3Result)
                                {
                                    DisplayVariableARCList();
                                }
                                else
                                {
                                    Console.WriteLine("No solution change input");
                                }
                                result = true;
                            }
                            else if (arcConsistancy == "2")
                            {
                               checkarcConsistancy = false;
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        if(!AC3Result )
                        {
                           
                            continue;
                        }

                        Console.WriteLine("Please choose method to solve csp");
                        Console.WriteLine("1.BackTracking");
                        Console.WriteLine("2.Forward Checking");
                        Console.WriteLine("3.Full Look Ahead");
                        Console.WriteLine("other key : Exit");
                        string method = (Console.ReadLine());
                        // create class of logic class

                        switch (method)
                        {
                            case "1":

                                 d = DateTime.Now;
                                //backtrack algorithm
                                CommonBackTrack(0, false, 0);
                                 d1 = DateTime.Now;
                                //track time taken
                                milliSeconds = (d1 - d).Milliseconds;
                                //solution exist
                                if (solutionFound)
                                {
                                    //display result
                                    DisplayVariablePosition(0);
                                }
                                else
                                {
                                    Console.WriteLine("No solution..please change input");
                                }
                                break;

                            case "2":

                                //forward checking algorithm
                                 d = DateTime.Now;
                                //common backtrack
                                CommonBackTrack(0, false, 1);
                                 d1 = DateTime.Now;
                                //track time taken
                                milliSeconds = (d1 - d).Milliseconds;

                                if (solutionFound)
                                {
                                    //display result
                                    DisplayVariablePosition(1);
                                }
                                else
                                {
                                    Console.WriteLine("No solution..please change input");
                                }
                                break;
                            case "3":

                                //full look ahead algorithm
                                d = DateTime.Now;
                                //full look ahead algorithm
                                bool fulllookresult=FulllookAhead(0,false);
                                d1 = DateTime.Now;
                                //track time taken
                                milliSeconds = (d1 - d).Milliseconds;

                                if (solutionFound && nosolution<=1 && fulllookresult)
                                {
                                    //display result
                                    DisplayVariablePosition(2);
                                }
                                else
                                {
                                    Console.WriteLine("No solution.. please change input");
                                }
                                break;
                            case "4":
                                exit = true;
                                Environment.Exit(1);
                                break;
                            default:
                                exit = true;
                                Environment.Exit(1);
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("error in ChooseMethod" );
                }

            }

            //get input logic
            public void GetCSPInput()
            {
                try
                {

                    //getting input for number of variables
                    Console.WriteLine("Please enter number of variables (n) ");
                    noVariables = Convert.ToInt32(Console.ReadLine());
                    bool result = false;
                    while (!result)
                    {
                        Console.WriteLine("Please enter constraint tightness (p) (>0 and <1) ");
                        constraintTightness = float.Parse(Console.ReadLine());
                        if (constraintTightness >= 1 || constraintTightness == 0)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }

                    //getting input for constant a
                    result = false;
                    while (!result)
                    {
                        Console.WriteLine("Please enter constant (α) (>0 and <1)");
                        constantA = float.Parse(Console.ReadLine());
                        if (constantA >= 1 || constantA == 0)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }

                    //getting input for constant r
                    result = false;
                    while (!result)
                    {
                        Console.WriteLine("Please enter constant (r) (>0 and <1)  ");
                        constantR = float.Parse(Console.ReadLine());
                        if (constantR >= 1 || constantR == 0)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    Console.WriteLine("--------------------------------------------------------");

                    //domain size
                    domainSize = Convert.ToInt32(Math.Pow(noVariables, constantA));
                    
                    //number of constraint
                    noOfConstraint = Convert.ToInt32(((constantR * noVariables) * Math.Log(noVariables)));

                    //number of tuples
                    noOftuples = Convert.ToInt32(constraintTightness * (domainSize * domainSize));

                    Console.WriteLine("domainSize: " + domainSize);
                    Console.WriteLine("Number Of Constraints (rn ln n): " + noOfConstraint);
                    Console.WriteLine("number Of incompatible tuples: " + noOftuples);


                    //displaying variable list
                    Console.WriteLine("Variable List : ");
                    Console.Write("{");
                    for (int i = 0; i < noVariables; i++)
                    {
                        variablesList.Add("X" + i);
                        CSPVariableModel cspVariableModel = new CSPVariableModel();
                        cspVariableModel.VariableName = i;
                        CSPVariableModelList.Add(cspVariableModel);
                        Console.Write(variablesList[i]);
                        if (i != noVariables - 1)
                        {
                            Console.Write(",");
                        }
                        else
                        {
                            Console.Write("}");
                            Console.WriteLine(" ");
                        }
                    }

                    //displaying domain list
                    Console.WriteLine("Domain List : ");
                    Console.Write("{");
                    for (int i = 0; i < domainSize; i++)
                    {

                        domainList.Add(i);
                        Console.Write(domainList[i]);
                        if (i != domainSize - 1)
                        {
                            Console.Write(",");
                        }
                        else
                        {
                            Console.Write("}");
                            Console.WriteLine(" ");
                        }
                    }
                    foreach (var item in CSPVariableModelList)
                    {
                        item.ArcDomainList = new List<int>();
                        item.ArcDomainList = domainList;
                    }
                    Console.WriteLine("--------------------------------------------------------");
                }
                catch (Exception)
                {
                    Console.WriteLine("wrong input given ..please give correct input" );
                    Console.WriteLine("---------------------------------------------------");

                    GetCSPInput();
                }
            }

            //generate random csp
            public void GenerateRandomCSP()
            {
                try
                {
                    //generating constraints list
                    Console.WriteLine("Constraint List (may take while to generate): ");
                    for (int i = 0; i < noOfConstraint; i++)
                    {
                        int count = 0;
                        int maxRandom = noVariables;
                        Random randomNo = new Random();
                        CSPConstraintsModel cspModel = new CSPConstraintsModel();
                        int random = randomNo.Next(0, maxRandom);
                        int nextRandom = randomNo.Next(0, maxRandom);
                        cspModel.FirstRandom = random;
                        cspModel.SecondRandom = nextRandom;

                        //check if constraint exist
                        count = cspConstraintModelList
                          .Where(x =>
                       (x.FirstRandom == cspModel.FirstRandom && x.SecondRandom == cspModel.SecondRandom)
                          || (x.FirstRandom == cspModel.SecondRandom && x.SecondRandom == cspModel.FirstRandom)).Count();

                        if (count == 0 && (cspModel.FirstRandom != cspModel.SecondRandom))
                        {
                            cspConstraintModelList.Add(cspModel);
                        }
                        else
                        {
                            i = i - 1;
                        }
                    }

                    //generating  incompitple tuple
                    foreach (var item in cspConstraintModelList)
                    {
                        item.CSPTupleList = new List<CSPTupleModel>();
                        for (int i = 0; i < noOftuples; i++)
                        {
                            int count = 0;
                            Random randomNo = new Random();
                            CSPTupleModel cspTupleModel = new CSPTupleModel();
                            int random = randomNo.Next(0, domainSize);
                            int nextRandom = randomNo.Next(0, domainSize);
                            cspTupleModel.FirstTuple = random;
                            cspTupleModel.SecondTuple = nextRandom;

                            //check if tuple exist
                            count = item.CSPTupleList
                            .Where(x =>
                              (x.FirstTuple == cspTupleModel.FirstTuple && x.SecondTuple == cspTupleModel.SecondTuple)).Count();

                            if (count == 0)
                            {
                                item.CSPTupleList.Add(cspTupleModel);
                            }
                            else
                            {
                                i = i - 1;
                            }
                        }
                    }

                  
                    foreach (var item in CSPVariableModelList)
                    {
                        item.ArcDomainList = new List<int>();
                        item.ArcDomainList = domainList;
                    }

                    //cspConstraintModelList = new List<CSPConstraintsModel>();
                    //CSPConstraintsModel cspmodel = new CSPConstraintsModel();
                    //cspmodel.FirstRandom = 0;
                    //cspmodel.SecondRandom = 1;
                    //cspmodel.CSPTupleList = new List<CSPTupleModel>();
                    //CSPTupleModel csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);


                    // csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    ////cs

                    //cspConstraintModelList.Add(cspmodel);

                    //cspmodel = new CSPConstraintsModel();
                    //cspmodel.FirstRandom = 2;
                    //cspmodel.SecondRandom = 0;
                    //cspmodel.CSPTupleList = new List<CSPTupleModel>();
                    // csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);


                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    ////cs
                    //cspConstraintModelList.Add(cspmodel);

                    //cspmodel = new CSPConstraintsModel();

                    //cspmodel.FirstRandom = 1;
                    //cspmodel.SecondRandom = 2;
                    //cspmodel.CSPTupleList = new List<CSPTupleModel>();
                    // csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 0;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);


                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    ////cs
                    //cspConstraintModelList.Add(cspmodel);

                    //cspmodel = new CSPConstraintsModel();

                    //cspmodel.FirstRandom = 3;
                    //cspmodel.SecondRandom = 0;
                    //cspmodel.CSPTupleList = new List<CSPTupleModel>();
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 1;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);


                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 2;
                    //csptuple.SecondTuple = 2;
                    //cspmodel.CSPTupleList.Add(csptuple);
                    //csptuple = new CSPTupleModel();
                    //csptuple.FirstTuple = 0;
                    //csptuple.SecondTuple = 1;
                    //cspmodel.CSPTupleList.Add(csptuple);


                    //cspConstraintModelList.Add(cspmodel);










                }

                catch (Exception)
                {
                    Console.WriteLine("error in generate random csp ");
                }
            }

            //display constraints and incompatiple tuples
            public void DisplayCSP()
            {
                try
                {
                    foreach (var item in cspConstraintModelList)
                    {

                        Console.WriteLine(string.Format("(X{0},X{1}) - ", item.FirstRandom, item.SecondRandom));
                        foreach (var tuple in item.CSPTupleList)
                        {
                            Console.Write(string.Format("({0},{1}) , ", tuple.FirstTuple, tuple.SecondTuple));

                        }
                        Console.WriteLine(Environment.NewLine);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("error in display csp ");
                }
            }

            //check if csp is solvable
            public bool CheckPhaseTransition()
            {
                bool result = false;
                double pt = 0;
                double e = Math.E; // Get E constant
                double pow = constantA / constantR;
                try
                {
                    pt = 1 - Math.Pow(e, -pow);
                    if (constraintTightness < pt)
                    {
                        Console.WriteLine("p < pt generated csp is solvable.");
                        Console.WriteLine("Thanks for giving relevant input.");
                        Console.WriteLine("---------------------------------------------------");
                        result = true;
                    }
                    else if (noOfConstraint == 1)
                    {
                        Console.WriteLine("number of constraint is 1 can't generate random csp.");
                        Console.WriteLine("---------------------------------------------------");

                        result = false;
                    }
                    else
                    {
                        Console.WriteLine("p > pt generated csp is not solvable.");
                        Console.WriteLine("Please give different input.");
                        Console.WriteLine("---------------------------------------------------");
                        result = false;
                    }
                }
                catch (Exception)
                {

                    Console.WriteLine("error in check phase transition");
                }
                return result;
            }
            #endregion

            #region AC3Algorithm
            //arc consistancy algorithm
            public bool AC3Algorithm()
            {
                //arc consistancy algorithm
                try
                {
                    List<CSPVariableModel> cspVariableList = new List<CSPVariableModel>();
                    cspVariableList = CSPVariableModelList;

                    // for each variable in varable list
                    foreach (var item in cspVariableList)
                    {
                        //get all rules which consist variable and other varble greater than variable
                        List<CSPConstraintsModel> cspConstraintRulesList = new List<CSPConstraintsModel>();
                        cspConstraintRulesList = cspConstraintModelList.Where(x =>
                       ((x.FirstRandom == item.VariableName && (x.SecondRandom <= noVariables))
                       || ((x.FirstRandom <= noVariables) && x.SecondRandom == item.VariableName))
                       && (x.FirstRandom == item.VariableName || x.SecondRandom == item.VariableName)
                         ).ToList();

                        //check which domain not compatible
                        foreach (var domain in domainList)
                        {

                            //foreach on every rule in ruleslist
                            foreach (var rule in cspConstraintRulesList)
                            {
                                //check if varible is first random
                                bool firstRandom = rule.FirstRandom == item.VariableName ? true : false;

                                //number of tuples in one rule
                                int noOfTupleRules = 0;

                                //find number of tuples which contains domain
                                if (firstRandom == true)
                                {
                                    noOfTupleRules = rule.CSPTupleList.Where(a => a.FirstTuple == domain).Count();
                                }
                                else
                                {
                                    noOfTupleRules = rule.CSPTupleList.Where(a => a.SecondTuple == domain).Count();

                                }

                                // if all tuples contains domain and count is greater than domain size then not compatible 
                                if (noOfTupleRules >= domainSize)
                                {
                                    //if not compatible domain then remove domain
                                    item.ArcDomainList = item.ArcDomainList.Where(a => a != domain).ToList();
                                    

                                }

                            }

                        }

                    }
                    CSPVariableModelList = cspVariableList;
                    foreach (var item in CSPVariableModelList)
                    {
                        if(item.ArcDomainList.Count==0)
                        {
                            solutionFound = false;
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception)
                {

                    Console.WriteLine("error occured in Arc Consistancy algorithm");
                    return false;
                }


            }

            //display variable after arc consistancy
            public void DisplayVariableARCList()
            {
                try
                {
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("Domain Size after AC algorithm");
                    foreach (var item in CSPVariableModelList)
                    {
                        Console.WriteLine("X" + item.VariableName + " - ");
                        Console.Write("{");
                        for (int i = 0; i < item.ArcDomainList.Count; i++)
                        {


                            Console.Write(item.ArcDomainList[i]);
                            if (i != item.ArcDomainList.Count - 1)
                            {
                                Console.Write(",");
                            }
                            else
                            {
                                Console.Write("}");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                    Console.WriteLine("----------------------------------------------------------");


                }
                catch (Exception)
                {

                    Console.WriteLine("");
                   
                }
            }

            #endregion

            #region logic
            //common backtrack
            public bool CommonBackTrack(int no, bool backTrack, int typeOfSearch)
            {

                try
                {

                    //for all variables
                    for (int i = no; i < noVariables; i++)
                    {
                         
                        //to track no of nodes it searhced
                        iterationTaken = iterationTaken + 1;
                       
                        //if no backtrack then set position to 0th item
                        if (backTrack == false)
                        {
                            CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition
                                = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];
                        }

                        else if (backTrack == true)
                        {


                            //get variable position
                            int variableposition = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition;
                            
                            //get index of position in list
                            int index = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.IndexOf(variableposition);

                           
                            //if no solution found
                            if (i == 0 &&
                                index == CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.Count - 1)
                            {
                                nosolution = nosolution + 1;
                                if (nosolution > 1)
                                {
                                    solutionFound = false;
                                    Console.WriteLine("No solution.. Please change input");
                                        return false;

                                   
                                }
                            }

                            //if index not found
                            if (index < 0)

                            {
                                variableposition = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];
                                index = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.IndexOf(variableposition);

                            }

                            //if backtrack true then set position = position +1
                            if (index + 1 < CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.Count && oldVariable >= i)
                            {
                                CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[index + 1];
                            }
                            else
                            {
                                //if backtrack true but no domain to move position then backtrack again (recursion)

                                //set current variable position to 0th item
                                CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition =
                                    CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];

                                //backtrack if i > 0 
                                if (i > 0 && oldVariable >= i)
                                {
                                    oldVariable = i;
                                    backTrack = true;
                                    //track backtrack
                                    nobackTracks = nobackTracks + 1;
                                    if (i - 1 == 0 && typeOfSearch != 0)
                                    {
                                        ReviseArcConsistancy(0, 0, 1);
                                    }
                                    CommonBackTrack(i - 1, true, typeOfSearch);
                                }
                            }
                        }

                        //position set now check if position satisfies with all connecting nodes

                        //get position of current variable 
                        int position = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition;

                        //get domain list of current variable
                        List<int> currentVariableDomains = new List<int>();
                        currentVariableDomains = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList;

                        //to track backtracking satisfy result
                        bool result = false;


                        if (typeOfSearch == 0 && i > 0)//backtrack search
                        {
                            //call standard backtracking which checks if current variable position satisfies with all past variable position

                            result = StandardBacktracking(i, position, currentVariableDomains);
                        }
                        else if (typeOfSearch == 1)//forward checking search
                        {
                            result = ForwardChecking(i, position, currentVariableDomains);
                        }

                        if (!result && i > 0)
                        {
                            backTrack = true;
                            oldVariable = i;
                                if (!solutionFound)
                                {
                                    return false;
                                }
                                else
                                {
                                    //if i ==1 then revise all domains to original position and starts x0 with next position
                                    if (typeOfSearch == 1 && i - 1 == 0)
                                    {
                                        ReviseArcConsistancy(0, 0, 1);
                                    }
                                    //track backtrack
                                    nobackTracks = nobackTracks + 1;
                                    //backtrack if solution exist
                                    CommonBackTrack(i - 1, true, typeOfSearch);

                                }

                        }
                        if(result==false && i==0 && typeOfSearch==1)
                        {
                            solutionFound = false;
                            return false;
                        }
                        oldVariable = i;
                    }
                       if(nosolution<=1)
                    {
                        solutionFound = true;
                    }
                   
                }
                catch (Exception)
                {

                    //Console.WriteLine("error in common backtrack search "+ ex.ToString());
                    return false;
                }
                return false;
            }

            //standard backtrack logic
            public bool StandardBacktracking(int variable, int position, List<int> itemDomainList)
            {
                //to track result
                bool result = false;
                try
                {


                    //Checking consistency between current  and past nodes
                    List<CSPConstraintsModel> cspConstraintRulesList = new List<CSPConstraintsModel>();

                    //get all constraints which are greater than equal to zero and less than equal to current node
                    cspConstraintRulesList = cspConstraintModelList.Where(x =>
                   ((x.FirstRandom >= 0 && x.FirstRandom <= variable) && (x.SecondRandom >= 0 && x.SecondRandom <= variable)) && (x.FirstRandom == variable || x.SecondRandom == variable)
                     ).ToList();

                    //not a single rule then current domain or position is satify
                    if (cspConstraintRulesList.Count() == 0)
                    {
                        return true;
                    }

                    //to track if constraint contains domain
                    int count = 0;
                    int move = 0;

                    //get only those domains which are greater than current variable position 
                    foreach (var item in itemDomainList.Where(a => a >= position))
                    {
                        // to track number of nodes searched
                        iterationTaken = iterationTaken + 1;

                        //if loop cant return then check again
                        if (count == 0 && move > 0)
                        {
                            result = true;

                            foreach (var model in CSPVariableModelList.Where(a => a.VariableName == variable))
                            {
                                int index = itemDomainList.IndexOf(item);
                                model.VariablePosition = itemDomainList[index - 1];
                            }
                            break;
                        }

                        //to tack number of constraints checked
                        int variableCount = 0;

                        //check with all constraints
                        foreach (CSPConstraintsModel variablePair in cspConstraintRulesList)
                        {
                            variableCount = variableCount + 1;
                            count = 0;
                            move = move + 1;

                            //other variable with current variable
                            int variableToSearch = variablePair.FirstRandom != variable ? variablePair.FirstRandom : variablePair.SecondRandom;

                            //other variable position
                            int variablePostion = CSPVariableModelList.Where(a => a.VariableName == variableToSearch).Select(a => a.VariablePosition).FirstOrDefault();

                            //check if its first variable or second
                            if (variablePair.FirstRandom != variable)//here variable will be second tuple
                            {
                                count = variablePair.CSPTupleList.Where(a => (a.FirstTuple == variablePostion && a.SecondTuple == item)).Count();
                            }
                            else //here variable will be first tuple
                            {
                                count = variablePair.CSPTupleList.Where(a => (a.FirstTuple == item && a.SecondTuple == variablePostion)).Count();
                            }

                            //if count is zero and domain is last then domain satisfy
                            if (count == 0 && move > 0 && item == itemDomainList[ itemDomainList.Count - 1] && cspConstraintRulesList.Count == variableCount)
                            {
                                result = true;
                                foreach (var model in CSPVariableModelList.Where(a => a.VariableName == variable))
                                {
                                    model.VariablePosition = item;
                                }
                                break;
                            }

                            //count is greater than  this domain not satisfy check next domain
                            if (count > 0)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("error in standard backtracking");
                }
                return result;
            }

            //forward checking logic
            public bool ForwardChecking(int variable, int position, List<int> itemDomainList)
            {
                //to track result
                //bool result = false;
                try
                {

                    //Checking consistency between current  and future nodes
                    List<CSPConstraintsModel> cspConstraintRulesList = new List<CSPConstraintsModel>();
                    cspConstraintRulesList = cspConstraintModelList.Where(x =>
                   ((x.FirstRandom >= variable && x.FirstRandom <= noVariables) && (x.SecondRandom >= variable && x.SecondRandom <= noVariables)) && (x.FirstRandom == variable || x.SecondRandom == variable)
                     ).ToList();

                    int count = 0;
                    int move = 0;


                    //for all domains which are greater than current variable position
                    foreach (var item in itemDomainList.Where(a => a >= position).ToList())
                    {
                        //to track number of nodes searched
                        iterationTaken = iterationTaken + 1;
                        int variableCount = 0;

                        //for all rules in constraints
                        foreach (CSPConstraintsModel variablePair in cspConstraintRulesList)
                        {
                            variableCount = variableCount + 1;
                            count = 0;
                            move = move + 1;

                            //check variable and its position
                            int variableToSearch = variablePair.FirstRandom != variable ? variablePair.FirstRandom : variablePair.SecondRandom;
                            if (variablePair.FirstRandom != variable)
                            {
                                //get count of tuples where tuple contains item
                                count = variablePair.CSPTupleList.Where(a => a.SecondTuple == item).Count();

                            }
                            else
                            {
                                count = variablePair.CSPTupleList.Where(a => a.FirstTuple == item).Count();

                            }
                            if (count != 0)
                            {

                                //get all tuples where tuple contains item
                                List<CSPTupleModel> tupleList = new List<CSPTupleModel>();
                                if (variablePair.FirstRandom != variable)
                                {

                                    tupleList = variablePair.CSPTupleList.Where(a => a.SecondTuple == item).ToList();
                                }
                                else
                                {
                                    tupleList = variablePair.CSPTupleList.Where(a => a.FirstTuple == item).ToList();

                                }
                                //remove all domains contain tuple from other variable

                                foreach (var tuple in tupleList)
                                {
                                    foreach (var cspvar in CSPVariableModelList.ToList())
                                    {
                                        if (cspvar.VariableName == variableToSearch)
                                        {
                                            if (variablePair.FirstRandom != variable)
                                            {
                                                cspvar.ArcDomainList = cspvar.ArcDomainList.Where(a => a != tuple.FirstTuple).ToList();
                                            }
                                            else
                                            {
                                                cspvar.ArcDomainList = cspvar.ArcDomainList.Where(a => a != tuple.SecondTuple).ToList();
                                            }
                                        }
                                    }
                                }

                            }

                            //if removing variable reduce domain empty then break loop and take other domain
                            if (CSPVariableModelList.Where(a => a.VariableName == variableToSearch).FirstOrDefault().ArcDomainList.Count == 0
                                && item != itemDomainList[itemDomainList.Count() - 1])
                            {
                                ReviseArcConsistancy(variable, 0,1);
                                break;

                            }

                            //if removing variable reduce domain empty and no other current variable domain then revise and reutrn false
                            if (item == itemDomainList[itemDomainList.Count - 1]
                                && CSPVariableModelList.Where(a => a.VariableName == variableToSearch).FirstOrDefault().ArcDomainList.Count == 0)
                            {
                                ReviseArcConsistancy(variable, 0,1);
                                return false;
                            }

                            if (variableCount >= cspConstraintRulesList.Count)
                            {
                                
                                CSPVariableModelList.Where(a => a.VariableName == variable).FirstOrDefault().VariablePosition = item;
                                return true;
                            }
                        }
                    }

                    return true;

                }
                catch (Exception)
                {

                    return false;
                }
            }

            //full look ahead
            public bool FulllookAhead(int macVariable,bool backTrack)
            {
                try
                {
                    for (int i = macVariable; i < CSPVariableModelList.Count; i++)
                    {
                        if (nosolution < noVariables)
                        {
                            try
                            {

                                int variable = CSPVariableModelList[i].VariableName;
                                List<int> itemDomainList = new List<int>();
                                int position = CSPVariableModelList[i].VariablePosition;
                                itemDomainList = CSPVariableModelList[i].ArcDomainList;

                                //get index of position in list
                                int index = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.IndexOf(position);
                                if (index < 0)
                                {
                                    position = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];
                                }

                                if (backTrack == false)
                                {
                                    CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition
                                        = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];
                                }

                                else if (backTrack == true)
                                {


                                    //get variable position
                                    //int variableposition = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition;

                                    //get index of position in list
                                    index = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.IndexOf(position);

                                    //if no solution found
                                    if (i == 0 && index == CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.Count - 1)
                                    {
                                        nosolution = nosolution + 1;
                                        if (nosolution > 1)
                                        {
                                            Console.WriteLine("No solution.. Please change input");
                                            break;
                                        }
                                    }

                                    //if index not found
                                    if (index < 0)

                                    {
                                        position = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];
                                        index = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.IndexOf(position);

                                    }

                                    //if backtrack true then set position = position +1
                                    if (index + 1 < CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList.Count && oldVariable > i)
                                    {
                                        CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[index + 1];
                                        position = CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[index + 1];
                                    }
                                    else
                                    {
                                        //if backtrack true but no domain to move position then backtrack again (recursion)

                                        //set current variable position to 0th item
                                        CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().VariablePosition =
                                            CSPVariableModelList.Where(a => a.VariableName == i).FirstOrDefault().ArcDomainList[0];

                                        //backtrack if i > 0 
                                        if (i > 0 && oldVariable > i)
                                        {
                                            oldVariable = i;
                                            backTrack = true;
                                          
                                            //track backtrack
                                            nobackTracks = nobackTracks + 1;
                                            if (i - 1 == 0)
                                            {
                                                ReviseArcConsistancy(0, 0, 2);
                                            }
                                            ReviseArcConsistancy(i - 1, 0, 2);
                                            FulllookAhead(i - 1, true);
                                            return true;
                                        }
                                    }
                                }





                                //Checking consistency between current  and future nodes
                                List<CSPConstraintsModel> cspConstraintRulesList = new List<CSPConstraintsModel>();
                                cspConstraintRulesList = cspConstraintModelList.Where(x =>
                               ((x.FirstRandom >= variable && x.FirstRandom <= noVariables) && (x.SecondRandom >= variable && x.SecondRandom <= noVariables)) && (x.FirstRandom == variable || x.SecondRandom == variable)
                                 ).ToList();

                                int count = 0;
                                int move = 0;

                                bool foundSolution = false;

                                //for all domains which are greater than current variable position
                                foreach (var item in itemDomainList.Where(a => a >= position).ToList())
                                {
                                    if (foundSolution)
                                    {
                                        break;
                                    }
                                    //to track number of nodes searched
                                    iterationTaken = iterationTaken + 1;
                                    int variableCount = 0;

                                    if(cspConstraintRulesList.Count()==0)
                                    {

                                      //  solutionFound = true;
                                      if(variable==noVariables-1)
                                        {
                                            solutionFound = true;
                                        }
                                        break;
                                    }

                                    //for all rules in constraints
                                    foreach (CSPConstraintsModel variablePair in cspConstraintRulesList)
                                    {
                                        variableCount = variableCount + 1;
                                        count = 0;
                                        move = move + 1;

                                        //check variable and its position
                                        int variableToSearch = variablePair.FirstRandom != variable ? variablePair.FirstRandom : variablePair.SecondRandom;
                                        if (variablePair.FirstRandom != variable)
                                        {
                                            //get count of tuples where tuple contains item
                                            count = variablePair.CSPTupleList.Where(a => a.SecondTuple == item).Count();

                                        }
                                        else
                                        {
                                            count = variablePair.CSPTupleList.Where(a => a.FirstTuple == item).Count();

                                        }
                                        if (count != 0)
                                        {

                                            //get all tuples where tuple contains item
                                            List<CSPTupleModel> tupleList = new List<CSPTupleModel>();
                                            if (variablePair.FirstRandom != variable)
                                            {

                                                tupleList = variablePair.CSPTupleList.Where(a => a.SecondTuple == item).ToList();
                                            }
                                            else
                                            {
                                                tupleList = variablePair.CSPTupleList.Where(a => a.FirstTuple == item).ToList();

                                            }
                                            //remove all domains contain tuple from other variable

                                            foreach (var tuple in tupleList)
                                            {
                                                foreach (var cspvar in CSPVariableModelList.ToList())
                                                {
                                                    if (cspvar.VariableName == variableToSearch)
                                                    {
                                                        if (variablePair.FirstRandom != variable)
                                                        {
                                                            cspvar.ArcDomainList = cspvar.ArcDomainList.Where(a => a != tuple.FirstTuple).ToList();
                                                        }
                                                        else
                                                        {
                                                            cspvar.ArcDomainList = cspvar.ArcDomainList.Where(a => a != tuple.SecondTuple).ToList();
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                        //if removing variable reduce domain empty then break loop and take other domain
                                        if (CSPVariableModelList.Where(a => a.VariableName == variableToSearch).FirstOrDefault().ArcDomainList.Count == 0
                                            && item != itemDomainList[itemDomainList.Count() - 1])
                                        {
                                            ReviseArcConsistancy(variable, 0, 2);
                                            break;

                                        }

                                        //if removing variable reduce domain empty and no other current variable domain then revise and reutrn false
                                        if (item == itemDomainList[itemDomainList.Count - 1]
                                            && CSPVariableModelList.Where(a => a.VariableName == variableToSearch).FirstOrDefault().ArcDomainList.Count == 0)
                                        {
                                            ReviseArcConsistancy(variable - 1, 0, 2);
                                            oldVariable = i;
                                            FulllookAhead(i - 1, true);
                                            foundSolution = true;
                                            if(solutionFound)
                                            {
                                                return true;
                                            }
                                            backTrack = true;
                                            break;

                                        }

                                        if (variableCount >= cspConstraintRulesList.Count)
                                        {
                                            CSPVariableModelList.Where(a => a.VariableName == variable).FirstOrDefault().VariablePosition = item;
                                           // oldVariable = i;
                                            foundSolution = true;
                                            if(variable==noVariables-1)
                                            {
                                                solutionFound = true;
                                                return true;
                                            }
                                            break;
                                        }

                                        if(foundSolution==false && variable==0&& item == itemDomainList[itemDomainList.Count - 1]&&(variableCount >= cspConstraintRulesList.Count))
                                        {
                                            return false;
                                        }
                                    }
                                }



                            }
                            catch (Exception)
                            {

                                return false;
                            }
                        }
                        else
                        {
                            solutionFound = false;
                            break;
                        }
                    }
                }
                catch (Exception)
                {

                    Console.WriteLine("error in full look ahead");
                    return false;
                }
                if (nosolution > 1)
                {
                    solutionFound = false;
                }
                return true;
            }


            //revise arc consistancy logic
            public void ReviseArcConsistancy(int variable, int position,int typeofSearch)
            {
                try
                {
                    reviseConsistancy = reviseConsistancy + 1;
                    List<int> listofVariable = new List<int>();
                    List<int> listofReviseVariable = new List<int>();

                    //get all variable which are less than current variable
                    listofVariable = CSPVariableModelList.Where(a => a.VariableName < variable).Select(a => a.VariableName).ToList();
                    listofVariable.Sort();

                    //get all variable which are greater than current variable
                    listofReviseVariable = CSPVariableModelList.Where(a => a.VariableName >= variable).Select(a => a.VariableName).ToList();
                    listofReviseVariable.Sort();

                    if (typeofSearch == 1)
                    {
                        //restore all item domains to original domains
                        foreach (var item in CSPVariableModelList.Where(a => a.VariableName >= variable))
                        {
                            if (item.VariableName != 0)
                            {
                                item.ArcDomainList = domainList;
                                item.VariablePosition = item.ArcDomainList[0];
                            }
                        }
                    }
                    else
                    {
                        //restore all item domains to original domains
                        foreach (var item in CSPVariableModelList.Where(a => a.VariableName > variable))
                        {
                            if (item.VariableName != 0)
                            {
                                item.ArcDomainList = domainList;
                                item.VariablePosition = item.ArcDomainList[0];
                            }
                        }
                    }

                    //if arc consistancy true
                    if (checkarcConsistancy)
                    {
                        AC3Algorithm();
                    }

                    if (variable != 0)
                    { 
                    //for all variable
                    foreach (var item in listofVariable)
                    {
                        foreach (var nextvariable in listofReviseVariable)
                        {

                            //get all constraint for current variable and next future variable
                            List<CSPConstraintsModel> cspConstraintRulesList = new List<CSPConstraintsModel>();
                            cspConstraintRulesList = cspConstraintModelList.Where(x =>
                            ((x.FirstRandom == item && x.SecondRandom == nextvariable) || (x.FirstRandom == nextvariable && x.SecondRandom == item))).ToList();

                            foreach (CSPConstraintsModel variablePair in cspConstraintRulesList)
                            {
                                int count = 0;
                                //check position of variable
                                int variableToSearch = variablePair.FirstRandom != item ? variablePair.FirstRandom : variablePair.SecondRandom;
                                int itemPostion = CSPVariableModelList.Where(a => a.VariableName == item).Select(a => a.VariablePosition).FirstOrDefault();


                                //get only those tuples which contain item position
                                List<CSPTupleModel> cspTupleList = new List<CSPTupleModel>();
                                if (variablePair.FirstRandom != item)
                                {
                                    count = variablePair.CSPTupleList.Where(a => a.SecondTuple == itemPostion).Count();
                                    cspTupleList = variablePair.CSPTupleList.Where(a => a.SecondTuple == itemPostion).ToList();
                                }
                                else
                                {
                                    count = variablePair.CSPTupleList.Where(a => a.FirstTuple == itemPostion).Count();
                                    cspTupleList = variablePair.CSPTupleList.Where(a => a.FirstTuple == itemPostion).ToList();
                                }


                                if (count != 0)
                                {
                                    foreach (var tuple in cspTupleList)
                                    {
                                        foreach (var cspvar in CSPVariableModelList.ToList())
                                        {
                                            if (cspvar.VariableName == variableToSearch)
                                            {
                                                if (variablePair.FirstRandom != item)
                                                {
                                                    cspvar.ArcDomainList = cspvar.ArcDomainList.Where(a => a != tuple.FirstTuple).ToList();
                                                }
                                                else
                                                {
                                                    cspvar.ArcDomainList = cspvar.ArcDomainList.Where(a => a != tuple.SecondTuple).ToList();

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                        if (typeofSearch == 1)
                        {
                        foreach (var item in CSPVariableModelList.Where(a => a.VariableName >= variable))
                                                {
                                                    if (item.VariableName != 0)
                                                    {
                                                        item.VariablePosition = item.ArcDomainList[0];
                                                    }
                                                }

                        }
                        else
                        {
                            foreach (var item in CSPVariableModelList.Where(a => a.VariableName > variable))
                            {
                                if (item.VariableName != 0)
                                {
                                    item.VariablePosition = item.ArcDomainList[0];
                                }
                            }

                        }

                    }
                }
                catch (Exception)
                {
                 //   Console.WriteLine("error in ReviseArcConsistancy");
                }
            }
            #endregion

            #region display solution
            //display solution
            public void DisplayVariablePosition(int typeOfSearch)
            {
                try
                {
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine("VariableName" + " - " + "solution");
                    foreach (var item in CSPVariableModelList)
                    {
                        Console.WriteLine("X" + item.VariableName + "               -  " + item.VariablePosition);
                    }
                    if (typeOfSearch == 0)
                    {
                        Console.WriteLine("Binary CSP has been solved by backtracking method");
                    }
                    else if (typeOfSearch == 1)
                    {
                        Console.WriteLine("Binary CSP has been solved by forward checking method");
                    }
                    else
                    {
                        Console.WriteLine("Binary CSP has been solved by full look ahead  method");
                    }
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine("Details : ");
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine("Time Taken(in milliseconds) : " + milliSeconds);
                    Console.WriteLine("No. of Nodes Search : " + iterationTaken);
                    Console.WriteLine("No. of Backtracks : " + nobackTracks);
                    Console.WriteLine("No. of Revise Arc Consitancy Checked : " + reviseConsistancy);

                    Console.WriteLine("--------------------------------------------------------");

                }
                catch (Exception)
                {

                    Console.WriteLine("");
                }
            }

            #endregion

            #region models
            //models to store data
            public class CSPConstraintsModel
            {
                public int FirstRandom { get; set; }
                public int SecondRandom { get; set; }

                public List<CSPTupleModel> CSPTupleList { get; set; }
            }

            public class CSPTupleModel
            {
                public int FirstTuple { get; set; }
                public int SecondTuple { get; set; }

            }

            public class CSPVariableModel
            {
                public int VariableName { get; set; }
                public int VariablePosition { get; set; }
                public List<int> ArcDomainList { get; set; }
                public bool IsPosition { get; set; }

            }

            #endregion
        }

    }
}
