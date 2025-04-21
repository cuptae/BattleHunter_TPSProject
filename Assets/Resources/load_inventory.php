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

$sql = "SELECT inventory_data FROM inventory_table WHERE user_id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $user_id);
$stmt->execute();
$stmt->bind_result($inventory_json);
$stmt->fetch();

echo $inventory_json;
$conn->close();
?>
