# LapTrinhWeb_TuDienWeb

Cấu trúc dự án
WebsiteTuDien/
│
├── App_Data/                          # Database files
│   └── DictionaryDB.mdf              # SQL Server Database
│
├── App_Code/                          # Business Logic Layer
│   ├── DAL/                          # Data Access Layer
│   │   ├── DatabaseHelper.cs         # SQL Connection & Helper
│   │   ├── WordRepository.cs         # CRUD từ điển
│   │   ├── UserRepository.cs         # CRUD người dùng
│   │   └── HistoryRepository.cs      # Lịch sử tra cứu
│   │
│   ├── BLL/                          # Business Logic
│   │   ├── WordService.cs            # Logic xử lý từ điển
│   │   ├── UserService.cs            # Logic xử lý user
│   │   └── SearchService.cs          # Logic tìm kiếm
│   │
│   └── Models/                        # Entity Models
│       ├── Word.cs                    # Model từ vựng
│       ├── User.cs                    # Model người dùng
│       └── SearchHistory.cs           # Model lịch sử
│
├── Content/                           # Static files
│   ├── css/
│   │   ├── bootstrap.min.css
│   │   ├── style.css                  # Custom CSS
│   │   └── admin.css
│   │
│   ├── js/
│   │   ├── jquery.min.js             
│   │   ├── bootstrap.min.js
│   │   └── script.js                  # Custom JS
│   │
│   ├── images/
│   │   ├── logo.png
│   │   ├── icons/
│   │   └── backgrounds/
│   │
│   └── audio/                         # Audio phát âm
│       └── pronunciations/
│
├── Client/                            # Trang người dùng
│   ├── Default.aspx                   # Trang chủ
│   ├── Search.aspx                    # Tra cứu từ điển
│   ├── WordDetail.aspx                # Chi tiết từ vựng
│   ├── History.aspx                   # Lịch sử tra cứu
│   ├── Favorites.aspx                 # Từ yêu thích
│   ├── Login.aspx                     # Đăng nhập
│   ├── Register.aspx                  # Đăng ký
│   └── Profile.aspx                   # Thông tin cá nhân
│
├── Admin/                             # Trang quản trị
│   ├── Dashboard.aspx                 # Trang tổng quan
│   ├── ManageWords.aspx               # Quản lý từ điển
│   ├── AddWord.aspx                   # Thêm từ mới
│   ├── EditWord.aspx                  # Sửa từ
│   ├── ManageUsers.aspx               # Quản lý người dùng
│   ├── Statistics.aspx                # Thống kê
│   └── Import.aspx                    # Import từ file
│
├── UserControls/                      # Reusable controls
│   ├── Header.ascx                    # Header chung
│   ├── Footer.ascx                    # Footer chung
│   ├── SearchBox.ascx                 # Ô tìm kiếm
│   └── WordCard.ascx                  # Card hiển thị từ
│
├── MasterPages/                       # Master pages
│   ├── Client.Master                  # Master cho client
│   └── Admin.Master                   # Master cho admin
│
├── Utils/                             # Utilities
│   ├── SessionHelper.cs               # Quản lý Session
│   ├── SecurityHelper.cs              # Mã hóa, bảo mật
│   └── ValidationHelper.cs            # Validate dữ liệu
│
├── Web.config                         # Cấu hình website
└── Global.asax                        # Application events
