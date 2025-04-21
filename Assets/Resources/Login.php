<?php
$host = "localhost";
$db = "mygame";
$user = "root";
$pass = "";

// DB 연결
$conn = new mysqli($host, $user, $pass, $db);
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// POST 받은 값
$username = $_POST['username'];
$password = $_POST['password'];

// 사용자 확인
$sql = "SELECT * FROM users WHERE username='$username' AND password='$password'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    echo json_encode(["status"=>"success"]);
} else {
    echo json_encode(["status"=>"fail"]);
}

$conn->close();
?>