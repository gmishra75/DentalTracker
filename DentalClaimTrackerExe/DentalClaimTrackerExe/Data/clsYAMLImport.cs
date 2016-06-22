using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace C_DentalClaimTracker
{
    class clsYAMLImport
    {
        public clsYAMLImport() { }

        public void ImportFile(string filePath, int first_Order_ID)
        {
            StreamReader sr = new StreamReader(filePath);
            int currentParentID = 0;
            Dictionary<int, int> orderIDs = new Dictionary<int, int>();
            Dictionary<int, int> parentIDs = new Dictionary<int, int>();
            question q;
            string currentLine;
            currentLine = sr.ReadLine();
            while (currentLine != null)
            {
                
                string currentLineNoSpace = CommonFunctions.TrimSpaces(currentLine);
                if (currentLine.StartsWith("#"))
                {
                    // Comment. Ignore it, just move to next line
                }
                else
                {                    
                    if (currentLineNoSpace == "-")
                    {
                        if (!parentIDs.ContainsKey(currentLine.Length))
                        {
                            parentIDs.Add(currentLine.Length, currentParentID);
                            if (currentLine == "-")
                            {
                                orderIDs.Add(currentLine.Length, first_Order_ID);
                            }
                            else
                            {
                                orderIDs.Add(currentLine.Length, 0);
                            }
                        }

                        currentParentID = parentIDs[currentLine.Length];
                    }
                    else if (currentLineNoSpace.StartsWith("question"))
                    {
                        // TODO: Requires changes to the 02-rejected .yml file. Everywhere else should be OK
                        // issue is "question" being used in a weird spot
                        // Need to rearrange the way it works so that instead of being a drop down, there are a 
                        // number of Yes/No options followed by an "other" expand category

                        if (!orderIDs.ContainsKey(currentParentID))
                        {
                            orderIDs.Add(currentParentID, 0);
                        }

                        q = CreateCategoryQuestion(currentParentID, currentLineNoSpace, orderIDs[currentParentID]);

                        orderIDs[currentParentID] = orderIDs[currentParentID] + 1;

                        currentParentID = q.id;
                        if (!orderIDs.ContainsKey(currentParentID))
                        {
                            orderIDs.Add(currentParentID, 0);
                        }
                    }
                    else if (currentLineNoSpace == "choices")
                    {
                        // Seems like I can ignore it, but might be necessary for returning to other "levels" of questions
                    }
                    else if (currentLineNoSpace.StartsWith("-"))
                    {
                        CreateQuestion(currentParentID, currentLineNoSpace, orderIDs[currentParentID]);

                        orderIDs[currentParentID] = orderIDs[currentParentID] + 1;
                    }
                }


                currentLine = sr.ReadLine();
            } 
        }

        private void CreateQuestion(int parentID, string currentLine, int orderID)
        {
            question newQ = new question();
            string workingText = currentLine;
            workingText = workingText.Replace("==", "|");
            string[] splitText = workingText.Split("|".ToCharArray());
            // [0] = text, [1] = type, [2] = question text or if combo, drop down options

            if ((splitText.Length < 2) || (splitText.Length > 4))
            {
                System.Diagnostics.Debug.Print("Length is wrong (" + splitText.Length + ")");
                return;
            }
            
            newQ.order_id = orderID;
            newQ.parent = parentID;
            newQ.text = CommonFunctions.TrimSpaces(splitText[0].Replace("- ", ""));

            newQ.type = GetQuestionType(CommonFunctions.TrimSpaces(splitText[1]));

            
            if (newQ.type == question.QuestionTypes.MultipleChoice)
            {
                if (splitText.Length == 3)
                {
                    newQ.popup_question_text = newQ.text;
                }
                else
                    newQ.popup_question_text = CommonFunctions.TrimSpaces(splitText[2]);
            }
            else
            {
                if (splitText.Length > 2)
                    newQ.popup_question_text = CommonFunctions.TrimSpaces(splitText[2]);
                else
                    newQ.popup_question_text = newQ.text;
                
            }

            newQ.Save();

            if (newQ.type == question.QuestionTypes.MultipleChoice)
            {
                GenerateMultipleChoiceAnswers(splitText[splitText.Length - 1], newQ.id);
            }

            

            
        }

        private void GenerateMultipleChoiceAnswers(string answersToParse, int questionID)
        {
            string[] answers = answersToParse.Split(",".ToCharArray());
            multiple_choice_answer mca = new multiple_choice_answer();
            mca.question_id = questionID;

            for (int i = 0; i < answers.Length; i++)
            {
                mca.order_id = i;
                mca.answer_text = CommonFunctions.TrimSpaces(answers[i]);
                mca.Save();
            }
        }

        private question.QuestionTypes GetQuestionType(string text)
        {
            switch (text)
            {
                case "LARGE_TEXT":
                    return question.QuestionTypes.LargeText;
                case "TEXT":
                    return question.QuestionTypes.NormalText;
                case "COMBO":
                    return question.QuestionTypes.MultipleChoice;
                case "DATE":
                    return question.QuestionTypes.Date;
                case "EXPAND":
                    System.Diagnostics.Debug.Print("Found category");
                    return question.QuestionTypes.Category;
                case "BOOLEAN":
                    return question.QuestionTypes.YesNo;
                default:
                    System.Diagnostics.Debug.Print("Unknown category");
                    throw new Exception("Unknown category type in GetQuestionType: " + text);
            }
        }

        private question CreateCategoryQuestion(int parentID, string currentLine, int orderID)
        {
            question newQ = new question();
            string questionText = currentLine.Replace("question: ", "");
            questionText = questionText.Replace(" == EXPAND", "");

            newQ.parent = parentID;
            newQ.type = question.QuestionTypes.Category;
            newQ.text = questionText;
            newQ.order_id = orderID;
            System.Diagnostics.Debug.Print(newQ.order_id.ToString());
            newQ.Save();

            return newQ;
        }


    }
}
