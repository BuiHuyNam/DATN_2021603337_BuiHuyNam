using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.OrderDto
{
    public class UpdateOrderStatusDto
    {
        public int Id { get; set; }
        public int Status { get; set; }

        //1. Chờ xác nhận
        //2. Đã xác nhận
        //3. Đã thanh toán
        //4. Đang giao hàng
        //5. Đã giao hàng
        //6. Đã Hủy
        //7. Yêu cầu trả hàng
    }
}
