using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HotelReservation.Data.Helpers
{
    public static class OrderByExtension
    {
        public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> query, string sortField, bool ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");

            Expression body = param;
            foreach (var member in sortField.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            var exp = Expression.Lambda(body, param);
            var method = ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = { query.ElementType, exp.Body.Type };
            var result = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(result);
        }
    }
}
