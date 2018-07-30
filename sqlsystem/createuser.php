<?php
	$servername = "localhost";
	$server_username = "root";
	$server_password = "";
	$dbname = "sqlsystem";

	$username = $_POST["username_Post"]; // waiting for username to be sent
	$email = $_POST["email_Post"];
	$password = $_POST["password_Post"];
	
	// make connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbname);
	//check connection
	if(!$conn)
	{
		die("Connection Failed.".mysql_connect_error());
	}
	// Make sure username is within database
	$checkuser =  "SELECT username FROM users";
	$checkuserresult = mysqli_query($conn, $checkuser);
	
	if(mysqli_num_rows($checkuserresult) > 0)
	{
		while($row = mysqli_fetch_assoc($checkuserresult))
		{
			if($row["username"] == $username)
			{
				print "User Already Exists";
			}
			else
			{
				$debug = "check email";
				print "check email";
			}
				
		}
	}
	else
	{
		$createuser = "INSERT INTO users(username, email, password) VALUES('".$username.";','".$email."', '".$password."')";
		$createuserresult = mysqli_query($conn, $createuser);
		if(!$createuserresult)
		{
			print "Error";
		}
		else
		{
			print "This is fine";
		}
	}
?>