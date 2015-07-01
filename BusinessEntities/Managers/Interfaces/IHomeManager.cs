using AJAD.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJAD.Business.Managers.Interfaces
{
    public interface IHomeManager
    {
        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns></returns>
        ActionOutput<APICountry> GetCountries();

        /// <summary>
        /// Get States by CountryID
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns></returns>
        ActionOutput<APIStates> GetStates(/*int CountryID*/);

        /// <summary>
        /// Log Exception into database
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        string LogExceptionToDatabase(Exception ex);

        /// <summary>
        /// Get All jokes
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="SearchKeywords"></param>
        /// <returns></returns>
        PagingResult<JokesViewModel> GetAllJokes(int PageNumber, int TotalRecords, string[] SearchKeywords);

        ActionOutput<CommentViewModel> GetAllComments(long JokeID);
    }
}
