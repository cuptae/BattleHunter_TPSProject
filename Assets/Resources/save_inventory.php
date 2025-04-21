<?php
$host = "localhost";
$db = "game_db";
$user = "db_user";
$pass = "db_password";

$conn = new mysqli($host, $user, $pass, $db);
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$user_id = $_POST['user_id'];
$inventory_json = $_POST['inventory_json'];

// 기존 데이터 업데이트 또는 삽입
$sql = "REPLACE INTO inventory_table (user_id, inventory_data) VALUES (?, ?)";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $user_id, $inventory_json);
$stmt->execute();

echo "OK";
$conn->close();
?>