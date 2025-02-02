using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace OrderTester
{
    public partial class MainForm : Form
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5000/api/orders";

        public MainForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void BtnSubmitOrder_ClickAsync(object sender, EventArgs e)
        {
            var order = new Order
            {
                OrderId = txtOrderId.Text,
                MenuItem = txtMenuItem.Text,
                Quantity = (int)numQuantity.Value,
                Status = "대기 중"
            };

            string json = JsonSerializer.Serialize(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(ApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("주문이 성공적으로 전송되었습니다!", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("주문 전송 실패! 서버 응답: " + response.StatusCode, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버와의 연결에 실패했습니다!\n" + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public string MenuItem { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
