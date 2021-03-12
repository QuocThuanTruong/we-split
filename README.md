<p align="center"><img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/logo.png" height="180"/></p>

# ĐỒ ÁN 2: WE SPLIT

<p align="left">
<img src="https://img.shields.io/badge/version-1.0.0-blue">
<img src="https://img.shields.io/badge/platforms-Windows-orange.svg">
</p>

### Thông tin nhóm

|       Họ và tên      |   MSSV   | Email                           | 
|----------------------|:--------:|---------------------------------|
| Trương Quốc Thuận    | 18120583 | lalalag129@gmail.com            |
| Hoàng Thị Thùy Trang | 18120605 | hoangthithuytrang1707@gmail.com |
| Lê Nhật Tuấn         | 18120632 | nhattuannhat99@gmail.com        |

### Các chức năng đã làm được
**1. Splash Screen (0.5 điểm)**
- Hiển thị thông tin chào mừng mỗi khi ứng dụng khởi chạy.
- Mỗi lần hiện ngẫu nhiên một thông tin thú vị về một địa điểm du lịch.
- Cho phép chọn check “Không hiện hộp thoại này mỗi khi khởi động”. Từ nay về sau đi thẳng vào màn hình HomeScreen luôn.

<div>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/splash.png" width="516"/>
</div>

**2. HomeScreen (4 điểm)**
- Liệt kê danh sách các chuyến đi, phân ra theo 2 loại: đã từng đi trước đó và đang đi.
- Xem chi tiết các chuyến đi: 
    + Danh sách các thành viên.
    + Danh sách các địa điểm, tổng kết
    các mục thu chi của cả nhóm (vẽ biểu đồ hình bánh)

<div>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Home_1.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Home_2.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Home_3.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Home_4.png" width="1080"/>
</div>

**3. SearchScreen (2 điểm)**
- Tìm kiếm chuyến đi theo tên địa điểm, tên thành viên trong chuyến đi. 
- Cảnh giới 1: Tìm chính xác mới chịu.
- Cảnh giới 2: Hỗ trợ tìm kiếm không dấu (cách dễ nhất, chuyển tất cả thành không dấu).
- Cảnh giới 3: Tìm không dấu hay có dấu hoặc có dấu chưa đúng nhưng kết quả vẫn ra và có độ ưu tiên.
- Ví dụ: Tìm với chữ “cỏ trang” vẫn có thể ra “cổ trang” và “cò trắng”.
- Cảnh giới 4: Tìm với từng từ hoặc kết hợp tạo ra tổ hợp từ các từ, có thể trong các trường khác nhau của dữ liệu.
- Cảnh giới 5: Thêm các từ khóa and, or, not.
- Cảnh giới 6: Dùng CSDL hỗ trợ sạch các cảnh giới trên (Nhóm sử dụng SQL Server Express 2019).

<div>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Search_1.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Search_2.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Search_3.png" width="1080"/>
</div>

**4. DetailScreen (2 điểm)**
- Cho phép trưởng nhóm tạo mới một chuyến đi với các thông tin
    + Tên chuyến đi
    + Thêm các thành viên
    + Thêm các khoản chi
    
    <div>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Detail_1.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Detail_2.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Detail_3.png" width="1080"/>
</div>

**5. UpdateJourneyScreen (2.5 điểm)**
- Cập nhật thông tin của chuyến đi: các thành viên, các hình ảnh, các mốc lộ trình, các khoản thu
chi. Chú ý nếu có người ứng trước cần báo cáo ai phải trả cho ai bao nhiêu tiền.

    <div>
        <img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Add_1.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Update.png" width="1080"/>
 <img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Site.png" width="1080"/>
</div>

**6. Others page**
- Help.
- About us.

   <div>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/Help.png" width="1080"/>
<img src="https://github.com/QuocThuanTruong/WeSplit/blob/master/Img/About.png" width="1080"/>
</div>

### Các chức năng chưa làm được theo yêu cầu của thầy
- **Không có**

### Các chức năng, đặc điểm đặc sắc của bài tập đề nghị cộng điểm
**Hiển thị thông tin lộ trình chi tiết kết hợp với Bing map và GoogleMapAPI**
- Thông tin lộ trình chi tiết được hiển thị bằng một dialog bao gồm thông tin chi tiết và một Map View được gắn các marker địa điểm muốn đến và vẽ lộ trình trên đó. 

**Hiển thị hình ảnh chuyến đi dạng carousel**
**Thực hiện toàn bộ các cảnh giới tìm kiếm với Full-Text search của SQL Server Express 2019**

### Điểm đề nghị cho bài tập
- **Điểm đề nghị: 10đ.**
- **Điểm cộng đề nghị: 1đ.**

### Link youtube demo
> ***https://www.youtube.com/watch?v=bOr5t7jbtw8

### Link drive bài nộp
> ***https://drive.google.com/file/d/1Y2vdHD89AyZDN5_kXV9R5Za8rvOQ48SE/view?usp=sharing

### Link github
> ***https://github.com/QuocThuanTruong/WeSplit

### Link backup file bài nộp
> ***https://drive.google.com/drive/folders/1cwaVwCftn_lZpmgCPqLc5zchLsnRxqjs?usp=sharing

### License
We Split is available under the [MIT license](https://opensource.org/licenses/MIT) . See [LICENSE](https://github.com/QuocThuanTruong/WeSplit/blob/master/LICENSE) for the full license text.
