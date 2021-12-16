using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.DAO;

namespace BLL
{
    public class PositionBLL
    {
        public static void AddPosition(POSITION position)
        {
            DAL.DAO.PositionDAO.AddPosition(position);
        }

        public static List<PositionDTO> GetPosition()
        {
            return DAL.DAO.PositionDAO.GetPosition();
        }

        public static void UpdatePosition(POSITION position, bool control)
        {
            PositionDAO.UpdatePosition(position);
            if (control)
                EmployeeDAO.UpdateEmployee(position);

        }

        public static void DeletePosition(int iD)
        {
            PositionDAO.DeletePosition(iD);
        }
    }
}
